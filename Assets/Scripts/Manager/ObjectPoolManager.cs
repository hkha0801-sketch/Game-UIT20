using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(prefab))
        {
            poolDictionary.Add(prefab, new Queue<GameObject>());
        }

        GameObject objToSpawn = null;

        if (poolDictionary[prefab].Count > 0)
        {
            objToSpawn = poolDictionary[prefab].Dequeue();
        }

        if (objToSpawn == null)
        {
            objToSpawn = Instantiate(prefab);
            PooledObject pooledObj = objToSpawn.AddComponent<PooledObject>();
            pooledObj.originalPrefab = prefab;
        }

        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;
        objToSpawn.SetActive(true);

        return objToSpawn;
    }

    public void ReturnObject(GameObject prefab, GameObject obj)
    {
        obj.SetActive(false);
        poolDictionary[prefab].Enqueue(obj);
    }
}