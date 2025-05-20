using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private int maxHealth;
    private int health;

    [SerializeField] private EnemyDeath enemyDeath;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            enemyDeath.DeathEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
