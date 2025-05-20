using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_EnemySpawner : MonoBehaviour
{
    public static ObjectPool_EnemySpawner instance;

    private List<GameObject> pooledObjects = new List<GameObject>();
    //[SerializeField] private int amountToPool; //Ej 20

    [SerializeField] private EnemySpawner enemySpawner;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < enemySpawner.maxEnemies; i++)
        {
            GameObject obj = Instantiate(enemySpawner.enemyPrefabs);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0;i < pooledObjects.Count; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }    
}
