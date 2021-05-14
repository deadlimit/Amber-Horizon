using System;
using EventCallbacks;
using UnityEngine;

[Serializable]
public class Checkpoint : MonoBehaviour {
    public static Checkpoint ActiveCheckPoint { get; private set; }
    
    private Transform player;
    
    public Vector3 SpawnPosition { get; private set; }
    
    [SerializeField] private AudioClip activateAudioClip;
    [SerializeField] private Color activeColor, inactiveColor;
    [SerializeField] private ParticleSystem activeIndicatorVFX;
    
    private void Awake() {
        SpawnPosition = transform.GetChild(0).transform.position;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player") || ActiveCheckPoint == this) return;

        if (player == null)
            player = other.transform;
        
        EventSystem<CheckPointActivatedEvent>.FireEvent(new CheckPointActivatedEvent(this, activateAudioClip));
        ActiveCheckPoint = this;
        EventSystem<DisplayUIMessage>.FireEvent(new DisplayUIMessage("Checkpoint activated", 2, false));
        enabled = false;
        ChangeParticleColor(true);  //Gör ändringen på färgen här
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
            var main = activeIndicatorVFX.main;
            main.startColor = inactiveColor;
        }
    }
    
    private void UpdateCheckPoint(Checkpoint newCheckpoint) {
        
        Checkpoint point = newCheckpoint;
        
        //här sätts activecheckpoint till röd
        ActiveCheckPoint.ChangeParticleColor(false);
        if(ActiveCheckPoint)
            ActiveCheckPoint.gameObject.SetActive(true);
        
        ActiveCheckPoint = point;
        //här sätts den nya activecheckpoint till grön
        ActiveCheckPoint.ChangeParticleColor(true);
        
        ActiveCheckPoint.gameObject.SetActive(false);
    }
    
    public void ResetPlayerPosition() {
        player.position = ActiveCheckPoint.SpawnPosition;
    }
}
