using System.Collections;
using EventCallbacks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;


public class SceneTransit : MonoBehaviour {

    public string NewSceneName;

    [SerializeField] private Animator GateLevelOne;
    [SerializeField] private PlayableDirector cinematic;    
    private BoxCollider trigger;
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
        trigger.enabled = false;
        GateLevelOne.SetTrigger(CloseGateHash);
        StartCoroutine(NewSceneSequence());
    }

    private IEnumerator NewSceneSequence() {
        
        yield return new WaitForSeconds((float)cinematic.duration);
        
        yield return StartCoroutine(LoadNewScene());
        
    }
    
    
    private IEnumerator LoadNewScene() {
        yield return SceneManager.UnloadSceneAsync("Level 1");
        yield return SceneManager.LoadSceneAsync(NewSceneName, LoadSceneMode.Additive);
        
        EventSystem<NewLevelLoadedEvent>.FireEvent(null);
        
        cinematic.Stop();
        FindObjectOfType<PlayerController>().enabled = true;
        FindObjectOfType<ThirdPersonCamera>().enabled = true;
        
        
    }
}
