using EventCallbacks;
using UnityEngine;
using UnityEngine.Playables;

public class EndGameGatePanel : GateLock {

    [SerializeField] private PlayableDirector cinematic;

    private void Start() {
        if (!OpenDoorWithoutKeys) return;
        
        FireUnlockSequence();
    }
    
    protected override void FireUnlockSequence() {
        
        FindObjectOfType<PlayerController>().gameObject.SetActive(false);
        interaction.enabled = false;
        cinematic.Play();
    }
}
