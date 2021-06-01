using EventCallbacks;
using UnityEngine;
using UnityEngine.Playables;

public class EndGameGatePanel : GateLock {

    [SerializeField] private PlayableDirector cinematic;
    private void Start() {
        KeyList.Clear();
    }
    
    protected override void FireUnlockSequence() {
        interaction.enabled = false;
        
        FindObjectOfType<PlayerController>().gameObject.SetActive(false);
        
        cinematic.Play();
    }
}
