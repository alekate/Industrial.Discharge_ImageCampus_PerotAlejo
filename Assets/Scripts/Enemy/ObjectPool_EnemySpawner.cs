using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPool_EnemySpawner : MonoBehaviour, IPoolable
{
    public static ObjectPool_EnemySpawner instance;

    private List<GameObject> pooledObjects = new List<GameObject>();

    [SerializeField] private EnemySpawner enemySpawner;

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
        enemySpawner = FindObjectOfType<EnemySpawner>();

    }

    public void InitializePool()
    {
        pooledObjects.Clear();

        for (int i = 0; i < enemySpawner.maxEnemies; i++)
        {
            GameObject obj = Instantiate(enemySpawner.enemyPrefabs);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }


    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i] == null)
            {
                pooledObjects.RemoveAt(i);
                i--; // Ajustar el índice después de remover
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
        enemySpawner = FindObjectOfType<EnemySpawner>();

        // Limpia objetos destruidos
        pooledObjects.RemoveAll(obj => obj == null);

        // Re-crear el pool si está vacío
        if (pooledObjects.Count == 0)
        {
            InitializePool();
        }
    }


}
