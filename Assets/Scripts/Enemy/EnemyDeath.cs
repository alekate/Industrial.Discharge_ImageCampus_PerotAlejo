using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyDeath : MonoBehaviour
{
    public ParticleSystem enemyParticle;
    public GameObject enemyMesh;

    public GameObject pickup;

    private void Awake()
    {
        enemyMesh.SetActive(true);
    }
    public void DeathEnemy()
    {
        enemyMesh.SetActive(false);
        StartCoroutine(PlayParticlesAndDestroy());
    }

    private IEnumerator PlayParticlesAndDestroy()
    {
        if (enemyParticle != null)
        {
            ParticleSystem ps = Instantiate(enemyParticle, enemyParticle.transform.position, enemyParticle.transform.rotation);

            ps.Play();

            Destroy(ps.gameObject, ps.main.duration + 0.5f);
        }

        Instantiate(pickup, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
}
