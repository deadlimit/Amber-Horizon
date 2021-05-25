using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    
    [SerializeField] private GameSettings GameSettings;
    [SerializeField] private string baseSceneName, startGameSceneName, projectileScene;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Camera camera;
    [SerializeField] private Color transitionTargetColor;
    
    private void OnEnable() {
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
        StartCoroutine(LoadSceneAdditive(projectileScene));
    }

    private void OnDisable() {
        volumeSlider.onValueChanged.RemoveAllListeners();
    }

    private void ChangeVolume(float newValue) {
        GameSettings.MasterVolume = newValue;
    }
    
    public void OnStartGame() {
        StartCoroutine(LoadStartScene());
    }

    private IEnumerator LoadSceneAdditive(string sceneName) {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }
    
    private IEnumerator LoadStartScene() {

        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log("Start load");
        yield return StartCoroutine(LoadSceneAdditive(baseSceneName));
        Debug.Log("Done base scene");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(baseSceneName));
        
        yield return StartCoroutine(LoadSceneAdditive(startGameSceneName));
        EventSystem<ReturnPlayerControl>.FireEvent(null);
        yield return SceneManager.UnloadSceneAsync(currentSceneName);
        
        
        
        Resources.UnloadUnusedAssets();
    }

    
}
