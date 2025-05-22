using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPool_EnemyProjectile : MonoBehaviour, IPoolable
{
    public static ObjectPool_EnemyProjectile instance;

    private List<GameObject> pooledObjects = new List<GameObject>();

    [SerializeField] private GameObject gameObjectPooledPrefab;
    [SerializeField] private int amountToPool = 10;

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
        InitializePool();
    }

    public void InitializePool()
    {
        pooledObjects.Clear();

        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(gameObjectPooledPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }

        Debug.Log($"[Pool: {name}] Inicializados {pooledObjects.Count} proyectiles.");
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i] == null)
            {
                pooledObjects.RemoveAt(i);
                i--;
                continue;
            }

            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Limpiar referencias destruidas
        pooledObjects.RemoveAll(obj => obj == null);

        // Si se vació por completo, volver a instanciar
        if (pooledObjects.Count == 0)
        {
            InitializePool();
        }
    }
}
