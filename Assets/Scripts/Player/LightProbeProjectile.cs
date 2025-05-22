using UnityEngine;

public class LightProbeProjectile : BaseProjectile
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            enemy.TakeDamage(damage);
        }

        if (!other.CompareTag("Player"))
        {
            Deactivate();
        }
    }
}
