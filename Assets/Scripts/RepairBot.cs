using UnityEngine;

public class RepairBot : MonoBehaviour
{
    [Header("Repair Bot Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    public float RegenFactor => Mathf.Clamp01(currentHealth / maxHealth); // Entre 0 y 1

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        if (currentHealth <= 0)
        {
            // Pod�s agregar animaci�n de destrucci�n o desactivaci�n
            Debug.Log("RepairBot destruido");
        }
    }
}
