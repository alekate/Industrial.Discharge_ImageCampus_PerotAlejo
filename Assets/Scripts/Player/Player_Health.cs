using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class Player_Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth = 4;
    private bool isDead = false;
    private bool isInvulnerable = false;

    public float invulnerabilityDuration = 1f;
    public ParticleSystem sparksParticle;
    public Rigidbody rb;

    [SerializeField] private UIController uiController;

    private void Awake()
    {
        sparksParticle.Pause();
        currentHealth = maxHealth;

        if (uiController != null)
        { 
            uiController.UpdateHealth(currentHealth, maxHealth);
        }
    }

    private void Update()
    {
        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Dead();
            currentHealth = 0;
        }

        if (isDead && Input.GetKey(KeyCode.Space))
        {
            RestartScene();
        }
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Ground"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth > 0 && !isDead && !isInvulnerable)
        {
            currentHealth -= amount;
            sparksParticle.Play();

            if (uiController != null)
            { 
                uiController.UpdateHealth(currentHealth, maxHealth);
            }

            StartCoroutine(InvulnerabilityCoroutine());
        }
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;

        yield return new WaitForSeconds(invulnerabilityDuration);

        isInvulnerable = false;
    }

    private void Dead()
    {
        GetComponent<Player_Movement>().enabled = false;
        GetComponent<Player_Rotation>().enabled = false;
        rb.useGravity = true;
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
