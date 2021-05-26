using System;
using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControlLock : MonoBehaviour {

    [SerializeField] private List<MonoBehaviour> scripts;

    private void Awake() {

        if (!SceneManager.GetActiveScene().name.Equals("MainMenu")) {
            Destroy(gameObject);
            return;
        }
            
        
        EventSystem<ReturnPlayerControl>.RegisterListener(EnableControl);
        StartCoroutine(EnableScripts(false));
    }

    private void OnEnable() {
        EventSystem<ReturnPlayerControl>.RegisterListener(EnableControl);
    }

    private void OnDisable() {
        EventSystem<ReturnPlayerControl>.UnregisterListener(EnableControl);
    }
    
    private void EnableControl(ReturnPlayerControl control) {
        Debug.Log("Enable controls");
        StartCoroutine(EnableScripts(true));
    }
    
    private IEnumerator EnableScripts (bool value) {
        foreach (MonoBehaviour script in scripts) {
            script.enabled = value;
            yield return null;
        }
            
    }
}
