using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_PlayerShoot : MonoBehaviour
{
    public static ObjectPool_PlayerShoot instance;

    private List<GameObject> pooledObjects = new List<GameObject>();

   // public int amountToPool; //Ej 20
    public Player_Shoot playerShoot;

    [SerializeField] private GameObject LightProbePrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < playerShoot.maxLightProbes; i++)
        {
            GameObject obj = Instantiate(LightProbePrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
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

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }
}
