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
    
    /// <summary>
    /// InstantiateObjectInAdditiveScene flyttar spawnade objekt till scenen objectpool-objektet finns i. Ligger nu i en egen scen, kanske inte behövs längre...
    /// </summary>
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

        if (!objectPools.ContainsKey(tag)) {
            Debug.Log("Objectpool doesnt contain tag: " + tag + "\n");
           
            foreach(string s in objectPools.Keys)
                Debug.Log(s + "\n");
        }
        
        PoolObject objectToSpawn = objectPools[tag].Dequeue();
        objectToSpawn.gameObject.SetActive(true);
        objectToSpawn.Initialize(position, rotation);
        objectPools[tag].Enqueue(objectToSpawn);
        
        return objectToSpawn.gameObject;
    }

}
