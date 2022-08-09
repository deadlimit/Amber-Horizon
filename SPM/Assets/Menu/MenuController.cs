using System.Collections;
using EventCallbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    [SerializeField]
    private string nameOfLevelToLoad;

    private void Awake() {
        Cursor.ActivateCursor(true, CursorLockMode.Confined);
    }

    public void ChangeLevelToLoad(string levelToLoad) 
    {
        nameOfLevelToLoad = levelToLoad;
        Debug.Log("in MC. ChangeLevelToLoad. New level to load is: " + nameOfLevelToLoad);
    }

    public void StartGame() {
        StartCoroutine(LoadLevels());
    }

    private IEnumerator LoadLevels() {

        //CAS should be the main menu scene.
        Scene currentActiveScene = SceneManager.GetActiveScene();
        if (nameOfLevelToLoad == "Intro Cutscene")
        {
            

            Debug.Log("In MC, LoadLevels. Hypothetical spot for loading intro cutscene.");
            yield return SceneManager.LoadSceneAsync("Intro_Cutscene", LoadSceneMode.Additive);

            yield return SceneManager.UnloadSceneAsync(currentActiveScene);
        }
        else { 
        //then check nameOfLevelToLoad and load the appropriate scene. 
        yield return SceneManager.LoadSceneAsync("Level 1 V2", LoadSceneMode.Additive);

        yield return SceneManager.LoadSceneAsync("BaseScene", LoadSceneMode.Additive);
        
        yield return SceneManager.LoadSceneAsync("ProjectileScene", LoadSceneMode.Additive);

        
        //I think the level that is loaded should be the active scene. But who knows.
        Scene newActiveScene = SceneManager.GetSceneByName("BaseScene");


        SceneManager.SetActiveScene(newActiveScene);
        EventSystem<NewLevelLoadedEvent>.FireEvent(null);
        EventSystem<ActivatePlayerControl>.FireEvent(new ActivatePlayerControl(true));
        yield return SceneManager.UnloadSceneAsync(currentActiveScene);
        }

    }
    
    
    
    
}
