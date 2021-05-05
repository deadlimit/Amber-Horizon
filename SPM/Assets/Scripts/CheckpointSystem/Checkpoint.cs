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
    public int ID;

    public Vector3 SpawnPosition { get; set; }
    
    public Action<int> OnPlayerEnter;

    public ParticleSystem activeIndicatorVFX;

    [SerializeField]
    private Color activeColor, inactiveColor;

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

    public void ChangeParticleColor(bool isActive)
    {             
        //removes any active particles, so the color changes immediatly
        activeIndicatorVFX.Clear();
        if (isActive)
        {
            Debug.Log("in Checkpoint ChangeParticleColor. is active.");

            var main = activeIndicatorVFX.main;
            main.startColor = activeColor;

            //stops the particle system and then starts it, to get the emission burst at the start. nevermind
            activeIndicatorVFX.Stop();
            activeIndicatorVFX.Play();
        }
        else 
        {
            Debug.Log("in Checkpoint ChangeParticleColor. is not active.");
            //activeIndicatorVFX.main.startColor = activeColor;

            var main = activeIndicatorVFX.main;
            main.startColor = inactiveColor;
        }
    }
}
