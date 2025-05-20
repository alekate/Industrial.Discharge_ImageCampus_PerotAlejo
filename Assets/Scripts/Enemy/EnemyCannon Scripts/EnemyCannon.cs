using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class EnemyCannon : MonoBehaviour
{
    public float detectionRadius = 10f;
    public Transform cannonDirection; //Mesh que gira para mirar el jugador 
    public float fireInterval = 2f;
    public float projectileSpeed = 10f;

    [SerializeField] private Transform player;

    private float fireTimer;

    [SerializeField] private FSM_EnemyMovement fsm_EnemyMovement;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            fsm_EnemyMovement.currentState = FSM_EnemyMovement.State.Wait;

            // El cañón mira al jugador
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, player.position.z);
            cannonDirection.LookAt(targetPos);

            // Disparo con intervalo
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireInterval)
            {
                FireProjectile();
                fireTimer = 0f;
            }
        }
        else
        {
            //fsm_EnemyMovement.currentState = FSM_EnemyMovement.State.Move;
        }
    }

    void FireProjectile()
    {
        GameObject proj = ObjectPool_EnemyProjectile.instance.GetPooledObject();
        if (proj != null)
        {
            proj.transform.position = cannonDirection.position;
            proj.transform.rotation = cannonDirection.rotation;

            Projectile script = proj.GetComponent<Projectile>();
            if (script != null)
            {
                script.Init(cannonDirection.forward * projectileSpeed);
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
