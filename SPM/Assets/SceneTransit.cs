using System.Collections;
using EventCallbacks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


public class SceneTransit : MonoBehaviour {

    public string NewSceneName;

    [SerializeField] private Animator FrontGate;
    [SerializeField] private PlayableDirector cinematic;   
    
    private BoxCollider trigger;
    
    private static readonly int OpenHash = Animator.StringToHash("Open");
    private static readonly int CloseHash = Animator.StringToHash("Close");

    private void Awake() => trigger = GetComponent<BoxCollider>();
    
    private void OnEnable() {
        EventSystem<UnlockEvent>.RegisterListener(OpenFrontGate);
    }

    private void OnDisable() {
        EventSystem<UnlockEvent>.UnregisterListener(OpenFrontGate);
    }

    private void OpenFrontGate(UnlockEvent unlockEvent) {
        FrontGate.SetTrigger(OpenHash);
    }

    private void OnTriggerEnter(Collider other) {
        trigger.enabled = false;
        FrontGate.SetTrigger(CloseHash);
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
