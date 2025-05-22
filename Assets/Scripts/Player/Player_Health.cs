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
    [SerializeField] private SceneController sceneController;

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

            sceneController.timeRemaining = 0;
            sceneController.timerIsRunning = false;
            uiController?.LoseGameUI();

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (isDead && Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            sceneController.LoadGame();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground") && !isInvulnerable)
        {
            TakeDamage(1);
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
}
