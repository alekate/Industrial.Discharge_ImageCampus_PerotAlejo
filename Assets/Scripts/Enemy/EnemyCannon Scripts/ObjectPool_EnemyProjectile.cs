using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_EnemyProjectile : MonoBehaviour
{
    public static ObjectPool_EnemyProjectile instance;

    private List<GameObject> pooledObjects = new List<GameObject>();

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int amountToPool = 20;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.SetActive(false);

            if (obj.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnDeactivate();            }

            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                pooledObjects[i].SetActive(true);

                if (pooledObjects[i].TryGetComponent<IPoolable>(out var poolable))
                {
                    poolable.OnActivate();
                }

                return pooledObjects[i];
            }
        }

        return null;
    }
}
