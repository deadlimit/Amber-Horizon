using System;
using System.Collections;
using EventCallbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransit : MonoBehaviour {

    public string NewSceneName;

    [SerializeField] private Animator GateLevelOne, GateLevelTwo;

    private static readonly int CloseGateHash = Animator.StringToHash("CloseGate");
    private static readonly int OpenGateHash = Animator.StringToHash("OpenGate");

    private void OnEnable() {
        EventSystem<UnlockEvent>.RegisterListener(OpenFrontGate);
    }

    private void OnDisable() {
        EventSystem<UnlockEvent>.UnregisterListener(OpenFrontGate);
    }

    private void OpenFrontGate(UnlockEvent unlockEvent) {
        GateLevelOne.SetTrigger(OpenGateHash);
    }

    private void OnTriggerEnter(Collider other) {
        GateLevelOne.SetTrigger(CloseGateHash);
        StartCoroutine(LoadNewScene());
    }

    private IEnumerator LoadNewScene() {
        
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        
        yield return SceneManager.LoadSceneAsync(NewSceneName);
        
        GateLevelTwo.SetTrigger(OpenGateHash);
    }
}
