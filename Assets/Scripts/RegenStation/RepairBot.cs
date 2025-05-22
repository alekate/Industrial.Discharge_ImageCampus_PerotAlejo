using UnityEngine;

public class RepairBot : MonoBehaviour
{
    [Header("Repair Bot Settings")]
    public float maxHealth = 10f;
    public float currentHealth;

    public float RegenFactor => Mathf.Clamp01(currentHealth / maxHealth);

    [Header("Damage Feedback")]
    [SerializeField] private Renderer botRenderer;
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.2f;

    private Color originalColor;
    private Coroutine flashCoroutine;

    private void Start()
    {
        currentHealth = maxHealth;

        if (botRenderer != null)
            originalColor = botRenderer.material.color;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }

        if (botRenderer != null)
        {
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(FlashDamageColor());
        }
    }

    private System.Collections.IEnumerator FlashDamageColor()
    {
        botRenderer.material.color = damageColor;

        yield return new WaitForSeconds(flashDuration);

        botRenderer.material.color = originalColor;
    }
}
