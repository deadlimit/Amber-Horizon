using EventCallbacks;
using UnityEngine;
using UnityEngine.Playables;

public class EndGameGatePanel : GateLock {

    [SerializeField] private PlayableDirector cinematic;
    
    protected override void FireUnlockSequence() {
        
        FindObjectOfType<PlayerController>().gameObject.SetActive(false);
        interaction.enabled = false;
        cinematic.Play();
    }
}
