using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class EnemyCannon : MonoBehaviour
{
    public float detectionRadius = 10f;
    public Transform cannonDirection; //Mesh que gira para mirar el jugador 
    public float fireInterval = 2f;
    [SerializeField] private float projectileDuration = 3f;


    [SerializeField] private Transform player;

    private float fireTimer;

    [SerializeField] bool requiersFSM = true;
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
            if (requiersFSM)
            {
                fsm_EnemyMovement.currentState = FSM_EnemyMovement.State.Wait;
            }

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
    }

    void FireProjectile()
    {
        GameObject proj = ObjectPool_EnemyProjectile.instance.GetPooledObject();

        proj.transform.position = cannonDirection.position;
        proj.transform.rotation = cannonDirection.rotation;

        EnemyProjectile script = proj.GetComponent<EnemyProjectile>();
        if (script != null)
        {
            script.Initialize(cannonDirection.forward, projectileDuration);
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
