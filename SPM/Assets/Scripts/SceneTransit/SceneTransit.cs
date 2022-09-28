using System.Collections;
using EventCallbacks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

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
        if (SceneManager.GetSceneByName("Level 1 V2").isLoaded)
        {
            //Debug.Log("in SceneTransit. unloading Level 1 V2");
            yield return SceneManager.UnloadSceneAsync("Level 1 V2");
        }

        
        yield return SceneManager.LoadSceneAsync(NewSceneName, LoadSceneMode.Additive);
        
        cinematic.Stop();
        FindObjectOfType<PlayerController>().enabled = true;
        FindObjectOfType<ThirdPersonCamera>().enabled = true;

        EventSystem<NewLevelLoadedEvent>.FireEvent(null);
    }
}
