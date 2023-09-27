using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolGenerator : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;

    }

    public static PoolGenerator instance; 
    
    private Dictionary<string, Queue<GameObject>> _objectPool;
    public List<Pool> pools;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
          _objectPool = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objpool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objpool.Enqueue(obj); 
            }
            _objectPool.Add(pool.tag,objpool);
        }
        
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!_objectPool.ContainsKey(tag)) return null;
        GameObject objectToSpawn = _objectPool[tag].Dequeue();
        
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        /*IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawned();
        }*/
        _objectPool[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

    public IEnumerator Func(string tag, Vector3 position, Quaternion rotation)
    {
        SpawnFromPool(tag, position, rotation);
        yield return new WaitForSeconds(1f);
    }
}
