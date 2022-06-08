using System;
using System.Collections;
using EventCallbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControlLock : MonoBehaviour {
    
    private void Awake() {
        EnableScripts(false);
        EventSystem<ActivatePlayerControl>.RegisterListener(EnableControl);
    }

    private void Start() {

        //StartCoroutine(WaitUntilSceneLoad());
        EnableScripts(true);
    }

    private IEnumerator WaitUntilSceneLoad() {
        while (!SceneManager.GetSceneByName("Level 1").isLoaded)
            yield return null;
        
        EnableScripts(true);
    }
    
    private void OnEnable() {
        EventSystem<ActivatePlayerControl>.RegisterListener(EnableControl);
    }

    private void OnDisable() {
        EventSystem<ActivatePlayerControl>.UnregisterListener(EnableControl);
    }
    
    private void EnableControl(ActivatePlayerControl control) {
        bool value = control?.Activate ?? true;
        EnableScripts(value);
    }
    
    private void EnableScripts (bool value) {

        GetComponent<PlayerController>().enabled = value;
        GetComponent<PlayerAnimation>().enabled = value;
        GetComponent<PhysicsComponent>().enabled = value;
        FindObjectOfType<ThirdPersonCamera>().enabled = value;
        
            
        if (!value) {
            Animator animator = GetComponent<Animator>();
            animator.SetFloat("VelocityX", 0);
            animator.SetFloat("VelocityZ", 0);

            GetComponent<PhysicsComponent>().velocity = Vector3.zero;
        }
        
    }

    private void OnDestroy() {
        EventSystem<ActivatePlayerControl>.UnregisterListener(EnableControl);
    }
}
