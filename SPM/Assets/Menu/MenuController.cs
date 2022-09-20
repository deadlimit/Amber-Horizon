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

        PlayerPrefs.SetString("levelToLoad", levelToLoad);

        Debug.Log("in MenuController, ChangeLevelToLoad. levelToLoad is: " + PlayerPrefs.GetString("levelToLoad"));
    }

    public void StartGame() {
        StartCoroutine(LoadLevels());
    }

    private IEnumerator LoadLevels() {

        //CAS should be the main menu scene.
        Scene currentActiveScene = SceneManager.GetActiveScene();

        //if intro, load intro. 
        if (nameOfLevelToLoad.Equals("IntroCutscene"))
        {
            SceneManager.LoadScene("IntroCutscene");

        }
        //if ZTD, load ZTD
        else if (nameOfLevelToLoad.Equals("ZTDCutscene"))
        {
            SceneManager.LoadScene("ZTDCutscene");

        }


        //else load level async.
        else { 
        switch (nameOfLevelToLoad) 
        {
            case "Level 1 V2":
                yield return SceneManager.LoadSceneAsync("Level 1 V2", LoadSceneMode.Additive);
                break;
            case "Level 2 V2":
                yield return SceneManager.LoadSceneAsync("Level 2", LoadSceneMode.Additive);
                break;
        }

        //and load the basescene and projectilescene
         yield return SceneManager.LoadSceneAsync("BaseScene", LoadSceneMode.Additive);
         yield return SceneManager.LoadSceneAsync("ProjectileScene", LoadSceneMode.Additive);


        //I think the level that is loaded should be the active scene. But who knows. like Lv1 or Lv2.
        Scene newActiveScene = SceneManager.GetSceneByName("BaseScene");

        SceneManager.SetActiveScene(newActiveScene);
        EventSystem<NewLevelLoadedEvent>.FireEvent(null);
        EventSystem<ActivatePlayerControl>.FireEvent(new ActivatePlayerControl(true));
        }


        //I think this has to happen last. So it doesn't unload the scene while the script is still running.
        yield return SceneManager.UnloadSceneAsync(currentActiveScene);
        }

    }
    
    
    
    
