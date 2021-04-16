using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class Spawner : MonoBehaviour
    {
        public GameObject UnitPrefab;
        public delegate void OnUnitSpawnedDelegate(Health health);
        public event OnUnitSpawnedDelegate OnUnitSpawnedListeners;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SpawnUnit();
            }
        }

        void SpawnUnit()
        {
            Vector3 pos = new Vector3(Random.Range(0, 25), 0, Random.Range(0, 25));
            GameObject go = Instantiate(UnitPrefab, pos, Quaternion.identity);
            OnUnitSpawnedListeners?.Invoke(go.GetComponent<Health>());
        }
    }
}

