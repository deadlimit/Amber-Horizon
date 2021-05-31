using EventCallbacks;
using UnityEngine;

public class SoundController : MonoBehaviour {

    private AudioSource audio;

    private void Awake() {
        audio = GetComponent<AudioSource>();
        
    }
    
    private void OnEnable() {
        EventSystem<SoundEffectEvent>.RegisterListener(PlaySFX);
    }

    private void OnDisable() {
        EventSystem<SoundEffectEvent>.UnregisterListener(PlaySFX);
    }

    private void PlaySFX(SoundEffectEvent sfxEvent) {
        audio.PlayOneShot(sfxEvent.SFX);
    }
    
}
