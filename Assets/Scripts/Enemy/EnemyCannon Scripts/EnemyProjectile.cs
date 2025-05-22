using UnityEngine;

public class EnemyProjectile : BaseProjectile
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player_Health player = other.GetComponent<Player_Health>();
            player.TakeDamage(damage);
        }

        if (!other.CompareTag("Enemy")) 
        {
            Deactivate();
        }
    }
}
