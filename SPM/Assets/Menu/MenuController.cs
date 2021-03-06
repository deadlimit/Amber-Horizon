using System.Collections;
using EventCallbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    private void Awake() {
        Cursor.ActivateCursor(true, CursorLockMode.Confined);
    }

    public void StartGame() {
        StartCoroutine(LoadLevels());
    }

    private IEnumerator LoadLevels() {

        Scene currentActiveScene = SceneManager.GetActiveScene();
        
        yield return SceneManager.LoadSceneAsync("BaseScene", LoadSceneMode.Additive);
        
        yield return SceneManager.LoadSceneAsync("ProjectileScene", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Level 1", LoadSceneMode.Additive);

        Scene newActiveScene = SceneManager.GetSceneByName("BaseScene");

        SceneManager.SetActiveScene(newActiveScene);
        EventSystem<NewLevelLoadedEvent>.FireEvent(null);
        EventSystem<ActivatePlayerControl>.FireEvent(new ActivatePlayerControl(true));
        yield return SceneManager.UnloadSceneAsync(currentActiveScene);
        

    }
    
    
    
    
}
