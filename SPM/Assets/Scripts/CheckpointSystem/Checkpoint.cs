using System;
using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;

[Serializable]
public class Checkpoint : MonoBehaviour {

    
    public static List<Checkpoint> activeCheckpoints = new List<Checkpoint>();
    public static List<Checkpoint> activatedCheckpoints = new List<Checkpoint>();
    
   
    public int ID { get; set; }
    public Vector3 SpawnPosition { get; private set; }

    [SerializeField] private AudioClip activateAudioClip;
    [SerializeField] private Color activeColor, inactiveColor;
    [SerializeField] private ParticleSystem activeIndicatorVFX;

    private void OnEnable() => activeCheckpoints.Add(this);

    private void OnDisable() => activeCheckpoints.Remove(this);

    private void Awake() {
        SpawnPosition = transform.GetChild(0).transform.position;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
        
        EventSystem<CheckPointActivatedEvent>.FireEvent(new CheckPointActivatedEvent(activateAudioClip, ID));
        EventSystem<DisplayUIMessage>.FireEvent(new DisplayUIMessage("Checkpoint activated", 2, false));
        activatedCheckpoints.Add(this);
        enabled = false;
    }
    

    public void ChangeParticleColor(bool isActive)
    {             
        //removes any active particles, so the color changes immediatly
        activeIndicatorVFX.Clear();
        if (isActive)
        {
            var main = activeIndicatorVFX.main;
            main.startColor = activeColor;

            //stops the particle system and then starts it, to get the emission burst at the start. nevermind
            activeIndicatorVFX.Stop();
            activeIndicatorVFX.Play();
        }
        else 
        {
            //activeIndicatorVFX.main.startColor = activeColor;

            var main = activeIndicatorVFX.main;
            main.startColor = inactiveColor;
        }
    }
}
