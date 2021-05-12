using System.Collections.Generic;
using System;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    [Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> objectPool;
 
    public void Start() {
        objectPool = new Dictionary<string, Queue<GameObject>>();

        foreach(var pool in pools) {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.size; i++) {
                var go = Instantiate(pool.prefab);
                go.SetActive(false);
                objectPool.Enqueue(go);
            }

            this.objectPool.Add(pool.tag, objectPool);
        }
    }

    public void Spawn(string tag, Vector3 position) {
        GameObject objectToSpawn = objectPool[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = Quaternion.identity;

        objectPool[tag].Enqueue(objectToSpawn);
    }

}