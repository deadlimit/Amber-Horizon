using System;
using UnityEngine;

[Serializable]
public class Checkpoint : MonoBehaviour {
    
    public int ID { get; set; }

    public Action<int> OnPlayerEnter;
    
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
            
        OnPlayerEnter?.Invoke(ID);
    }
}
