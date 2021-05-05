using System;
using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Checkpoint : MonoBehaviour {

    public static readonly List<Checkpoint> activeCheckpoints = new List<Checkpoint>();
    public static readonly List<Checkpoint> activatedCheckpoints = new List<Checkpoint>();
    
    public AudioClip activateAudioClip;

    public int ID { get; set; }

    public Vector3 SpawnPosition { get; set; }
    
    public Action<int> OnPlayerEnter;

    private void OnEnable() => activeCheckpoints.Add(this);

    private void OnDisable() => activeCheckpoints.Remove(this);

    private void Awake() {
        SpawnPosition = transform.GetChild(0).transform.position;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        
        OnPlayerEnter?.Invoke(ID);
        EventSystem<CheckPointActivatedEvent>.FireEvent(new CheckPointActivatedEvent(activateAudioClip));
        activatedCheckpoints.Add(this);

        enabled = false;
    }
}
