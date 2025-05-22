using UnityEngine;

public class RegenStation : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject outOfOrderText;
    [SerializeField] private GameObject regentStationText;

    [SerializeField] private UIController UIcontroller;

    [Header("RepairBot Settings")]
    [SerializeField] private RepairBot repairBot;

    [Header("Regen Multipliers")]
    [SerializeField][Range(0f, 1f)] private float baseRegenMultiplier = 1f;
    [SerializeField] private bool useRepairBotFactor = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Ocultamos todos los textos primero para evitar que queden prendidos
        regentStationText?.SetActive(false);
        outOfOrderText?.SetActive(false);

        if (useRepairBotFactor && repairBot != null && repairBot.RegenFactor <= 0f)
        {
            outOfOrderText?.SetActive(true);
            return; // No curar si el bot está destruido
        }

        // Mostrar texto de estación activa
        regentStationText?.SetActive(true);

        // Calcular y aplicar regeneración
        Player_Health player_Health = other.GetComponent<Player_Health>();
        Player_Shoot player_Shoot = other.GetComponent<Player_Shoot>();

        float regenFactor = baseRegenMultiplier;

        if (useRepairBotFactor && repairBot != null)
            regenFactor *= repairBot.RegenFactor;

        UIcontroller.PlayWhiteFlash();

        int newHealth = Mathf.FloorToInt(player_Health.maxHealth * regenFactor);
        int newBullets = Mathf.FloorToInt(player_Shoot.maxBullets * regenFactor);
        int newLightProbes = Mathf.FloorToInt(player_Shoot.maxLightProbes * regenFactor);

        if (newHealth > player_Health.currentHealth)
            player_Health.currentHealth = Mathf.Min(newHealth, player_Health.maxHealth);

        if (newBullets > player_Shoot.remainingBullets)
            player_Shoot.remainingBullets = newBullets;

        if (newLightProbes > player_Shoot.remainingLightProbes)
            player_Shoot.remainingLightProbes = newLightProbes;

        UIcontroller.UpdateHealth(player_Health.currentHealth, player_Health.maxHealth);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        regentStationText?.SetActive(false);
        outOfOrderText?.SetActive(false);
    }
}
