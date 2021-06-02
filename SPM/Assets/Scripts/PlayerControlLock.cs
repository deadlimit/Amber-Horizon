using System;
using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControlLock : MonoBehaviour {

    [SerializeField] private List<MonoBehaviour> scripts;

    private void Awake() {
        StartCoroutine(EnableScripts(false));
        EventSystem<ActivatePlayerControl>.RegisterListener(EnableControl);
    }

    private IEnumerator Start() {
        while (!SceneManager.GetSceneByName("Level 1").isLoaded)
            yield return null;
        
        StartCoroutine(EnableScripts(true));
    }

    private void OnEnable() {
        EventSystem<ActivatePlayerControl>.RegisterListener(EnableControl);
    }

    private void OnDisable() {
        EventSystem<ActivatePlayerControl>.UnregisterListener(EnableControl);
    }
    
    private void EnableControl(ActivatePlayerControl control) {
        if(gameObject.activeInHierarchy)
            StartCoroutine(EnableScripts(control?.Activate ?? true));
    }
    
    private IEnumerator EnableScripts (bool value) {
        foreach (MonoBehaviour script in scripts) {
            script.enabled = value;
            yield return null;
        }

        if (!value) {
            Animator animator = GetComponent<Animator>();
            animator.SetFloat("VelocityX", 0);
            animator.SetFloat("VelocityZ", 0);

            GetComponent<PhysicsComponent>().velocity = Vector3.zero;
        }
  
    }
}
