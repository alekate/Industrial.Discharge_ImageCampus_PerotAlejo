using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenStation : MonoBehaviour
{
    [SerializeField] private UIController uiController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiController.PlayWhiteFlash();

            Player_Health player_Health = other.GetComponent<Player_Health>();
            Player_Shoot player_Shoot = other.GetComponent<Player_Shoot>();

            player_Health.currentHealth = player_Health.maxHealth;

            player_Shoot.remainingBullets = player_Shoot.maxBullets;
            player_Shoot.remainingLightProbes = player_Shoot.maxLightProbes;

            uiController.UpdateHealth(player_Health.currentHealth, player_Health.maxHealth);

        }

    }
}
