using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_PlayerShoot : MonoBehaviour, IPoolable
{
    public static ObjectPool_PlayerShoot instance;

    private List<GameObject> pooledObjects = new List<GameObject>();

    [SerializeField] private GameObject lightProbePrefab;
    [SerializeField] private float amountToPool; // Valor por defecto

    public Player_Shoot playerShoot;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    { 
        amountToPool = playerShoot.maxLightProbes;
        InitializePool();
        playerShoot = FindObjectOfType<Player_Shoot>();
    }

    public void InitializePool()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(lightProbePrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        return null;
    }

    public int GetAvailableObjectsCount()
    {
        int count = 0;
        foreach (GameObject obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
                count++;
        }
        return count;
    }
}
