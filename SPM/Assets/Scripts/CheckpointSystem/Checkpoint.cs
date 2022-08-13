using EventCallbacks;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    private static Checkpoint activeCheckpoint;
    [SerializeField]
    private static Transform player;
    
    private BoxCollider trigger;
    [SerializeField] private bool isStartingCheckpoint;
    [SerializeField] private AudioClip activateAudioClip;
    [SerializeField] private Color activeColor, inactiveColor;
    [SerializeField] private ParticleSystem activeIndicatorPanelVFX;
    [SerializeField] private ParticleSystem activeIndicatorStandVFX;


    public Vector3 SpawnPosition { get; private set; }
    
    private void Awake() {
        SpawnPosition = transform.GetChild(0).transform.position;
        trigger = GetComponent<BoxCollider>();
        player = FindObjectOfType<PlayerController>().transform;
        
        if (isStartingCheckpoint) {
            if(activeCheckpoint != null)
                Debug.LogWarning("More than one checkpoint chosen as starting checkpoint");
            
            activeCheckpoint = this;
        }
        
            
    }
    
    private void OnTriggerEnter(Collider other) {
        if (activeCheckpoint != null) 
            activeCheckpoint.trigger.enabled = true;
        
        activeCheckpoint = this;
        activeCheckpoint.trigger.enabled = false;
        
        EventSystem<CheckPointActivatedEvent>.FireEvent(new CheckPointActivatedEvent(this, activateAudioClip));
        EventSystem<DisplayUIMessage>.FireEvent(new DisplayUIMessage("Checkpoint activated", 1, false));
        
        ChangeParticleColor(true);
    }
    
    
    public static void RespawnAtActiveCheckpoint() => player.position = activeCheckpoint.SpawnPosition;

    public void ChangeParticleColor(bool isActive)
    {
        //removes any active particles, so the color changes immediatly.
        activeIndicatorPanelVFX.Clear();
        activeIndicatorStandVFX.Clear();
        var mainPanel = activeIndicatorPanelVFX.main;
        var mainStand = activeIndicatorStandVFX.main;
        if (isActive)
        {           
            mainPanel.startColor = activeColor;
            mainStand.startColor = activeColor;
            
        }
        else 
        {
            mainPanel.startColor = inactiveColor;           
            mainStand.startColor = inactiveColor;
        }
    }
    
    
}
