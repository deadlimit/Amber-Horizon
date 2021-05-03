using System;
using EventCallbacks;
using UnityEngine;

[Serializable]
public class Checkpoint : MonoBehaviour {

    public AudioClip audio;
    public int ID { get; set; }

    public Vector3 SpawnPosition { get; set; }
    
    public Action<int> OnPlayerEnter;

    private void Awake() {
        SpawnPosition = transform.GetChild(0).transform.position;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
            
        
        OnPlayerEnter?.Invoke(ID);
        EventSystem<CheckPointActivatedEvent>.FireEvent(new CheckPointActivatedEvent(audio));
    }
}
