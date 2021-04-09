using System;
using UnityEngine;

[Serializable]
public class Checkpoint : MonoBehaviour {
    
    [HideInInspector] public int ID;

    public Action OnPlayerEnter;
    
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        
        OnPlayerEnter?.Invoke();
    }
}
