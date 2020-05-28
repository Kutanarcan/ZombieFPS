using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField]
    Poolnfo poolnfo;

    Dictionary<string, Queue<GameObject>> poolDictionary;
    Dictionary<string, GameObject> backupDictionary;

    GameObject objectContainer;

    public GameObject ObjectContainer
    {
        get
        {
            if (objectContainer == null)
            {
                objectContainer = new GameObject() { name = "ObjectContainer" };
            }

            DontDestroyOnLoad(objectContainer);
            return objectContainer;
        }
    }

    public static ObjectPooler Instance { get; private set; }

    GameObject poolObj;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        backupDictionary = new Dictionary<string, GameObject>();

        foreach (PoolInfoAsset pool in poolnfo.poolInfos)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                poolObj = Instantiate(pool.prefab);

                poolObj.name = pool.prefab.name;
                poolObj.transform.SetParent(ObjectContainer.transform);
                poolObj.SetActive(false);
                objectPool.Enqueue(poolObj);
            }
            poolDictionary.Add(pool.prefab.name, objectPool);
            backupDictionary.Add(pool.prefab.name, poolObj);

            poolObj = null;
        }
    }

    void AddObjectWhenPoolEmpty(string prefabName)
    {
        if (!backupDictionary.ContainsKey(prefabName))
        {
            Debug.LogError($"Pool Object : {prefabName} is not in Backup Dictionary.");
            return;
        }

        GameObject prefab = backupDictionary[prefabName];

        prefab.name = backupDictionary[prefabName].name;
        prefab.transform.SetParent(ObjectContainer.transform);
        prefab.SetActive(false);

        poolDictionary[prefabName].Enqueue(prefab);
    }

    public GameObject SpawnPoolObject(string prefabName, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefabName))
        {
            Debug.LogError($"Pool Object : {prefabName} is not a poolable object.");
            return null;
        }

        if (IsPoolEmpty(prefabName))
            AddObjectWhenPoolEmpty(prefabName);

        GameObject objectToSpawn = poolDictionary[prefabName].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    public void ReturnToPool(string prefabName, GameObject prefab)
    {
        if (!poolDictionary.ContainsKey(prefabName))
        {
            Debug.LogError($"Pool Object : {prefabName} cannot return to pool. Because is not in the pool list.");
            return;
        }

        StopDestroyCoroutine();
        prefab.SetActive(false);
        poolDictionary[prefabName].Enqueue(prefab);
    }

    Coroutine destroyCoroutine;
    WaitForSeconds destroySeconds;

    public void ReturnToPool(string prefabName, GameObject prefab, float destroyTime)
    {
        if (!poolDictionary.ContainsKey(prefabName))
        {
            Debug.LogError($"Pool Object : {prefabName} cannot return to pool. Because is not in the pool list.");
            return;
        }

        destroySeconds = new WaitForSeconds(destroyTime);
        destroyCoroutine = StartCoroutine(DestroyCoroutine());

        IEnumerator DestroyCoroutine()
        {
            yield return destroySeconds;
            prefab.SetActive(false);
            poolDictionary[prefabName].Enqueue(prefab);
        }
    }

    void StopDestroyCoroutine()
    {
        if (destroyCoroutine != null)
        {
            StopCoroutine(destroyCoroutine);
            destroyCoroutine = null;
        }
    }

    public bool IsPoolEmpty(string prefabName)
    {
        return poolDictionary[prefabName].Count <= 0;
    }
}
