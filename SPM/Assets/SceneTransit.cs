using System.Collections;
using EventCallbacks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransit : MonoBehaviour {

    public string NewSceneName;

    [SerializeField] private Animator GateLevelOne;
    [SerializeField] private BoxCollider trigger;
    private static readonly int CloseGateHash = Animator.StringToHash("CloseGate");
    private static readonly int OpenGateHash = Animator.StringToHash("OpenGate");

    private void Awake() => trigger = GetComponent<BoxCollider>();
    
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
        trigger.enabled = false;
        StartCoroutine(LoadNewScene());
        
    }

    private IEnumerator LoadNewScene() {
        
        yield return SceneManager.UnloadSceneAsync("Level 1", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        
        yield return SceneManager.LoadSceneAsync(NewSceneName, LoadSceneMode.Additive);
    }
}
