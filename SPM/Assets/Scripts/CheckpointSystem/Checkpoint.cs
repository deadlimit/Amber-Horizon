using System;
using UnityEngine;

[Serializable]
public class Checkpoint : MonoBehaviour {
    
    public int ID { get; set; }

    public Vector3 SpawnPosition;
    
    public Action<int> OnPlayerEnter;

    private void Awake() {
        SpawnPosition = transform.GetChild(0).transform.position;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
            
        OnPlayerEnter?.Invoke(ID);
    }
}
