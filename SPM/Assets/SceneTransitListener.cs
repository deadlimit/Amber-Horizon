using System;
using System.Linq;
using UnityEngine;
using EventCallbacks;
using UnityEngine.SceneManagement;

public class SceneTransitListener : MonoBehaviour {

    private void OnEnable() => EventSystem<SceneTransitEvent>.RegisterListener(ChangeScene);

    private void OnDisable() => EventSystem<SceneTransitEvent>.UnregisterListener(ChangeScene);

    private void ChangeScene(SceneTransitEvent eventInfo) {
        
        SceneManager.LoadScene(eventInfo.Scene);
    }
}
