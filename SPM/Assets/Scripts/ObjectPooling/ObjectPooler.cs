using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPooler : MonoBehaviour {
    
    [Serializable]
    public class Pool {
        public string tag;
        public PoolObject prefab;
        public int size;
    }

    private static ObjectPooler instance;
    
    public static ObjectPooler Instance => instance;
    
    public List<Pool> pools;

    private Dictionary<string, Queue<PoolObject>> objectPools;

    private void Awake() {
        instance = this;
        
    }
    
    public void Start() {
        objectPools = new Dictionary<string, Queue<PoolObject>>();

        foreach(var pool in pools) {
            Queue<PoolObject> objectPool = new Queue<PoolObject>();

            for(int i = 0; i < pool.size; i++) {
                PoolObject poolObject = Instantiate(pool.prefab);
                poolObject.InstantiateObjectInAdditiveScene(gameObject.scene);
                poolObject.gameObject.SetActive(false);
                objectPool.Enqueue(poolObject);
            }

            objectPools.Add(pool.tag, objectPool);
        }
    }

    public GameObject Spawn(string tag, Vector3 position, Quaternion rotation) {
        PoolObject objectToSpawn = objectPools[tag].Dequeue();
        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.Initialize(position, rotation);
        objectPools[tag].Enqueue(objectToSpawn);
        
        return objectToSpawn.gameObject;
    }

}
