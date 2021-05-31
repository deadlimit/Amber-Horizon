using System;
using EventCallbacks;
using UnityEngine;

public class AudioController : MonoBehaviour {

    private AudioSource audiosource;

    private void Awake() {
        audiosource = GetComponent<AudioSource>();
    }

    private void OnEnable() => EventSystem<CheckPointActivatedEvent>.RegisterListener(PlayAudio);

    private void OnDisable() => EventSystem<CheckPointActivatedEvent>.UnregisterListener(PlayAudio);
    private void PlayAudio(CheckPointActivatedEvent eventInfo) {
        audiosource.PlayOneShot(eventInfo.audio);
    }


}
