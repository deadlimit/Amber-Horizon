using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadSceneFromIntro : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textInUI;
    [SerializeField]
    private bool firstInputMade = false;

    [SerializeField]
    private GameObject canvasCover;

    //I don't want the player loading the next level at the same time as the game starts the load.
    //Could cause delay.
    //This value is changed by a signal in the intro cutscene.
    [SerializeField]
    private bool lateInIntro = false;

    private void Start()
    {
        textInUI.text = "Press Esc\nto skip.";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pressed ESC in intro.");
            FirstInput();

        }

        if (Input.GetKeyDown(KeyCode.Space) && !lateInIntro)
        {
            Debug.Log("Pressed space in intro.");
            StartCoroutine(LoadLevel1());
        }
    }

    public void LoadLevel1Caller() 
    {
        StartCoroutine(LoadLevel1());
        Debug.Log("End of intro cutscene.");
    }

    private IEnumerator LoadLevel1()
    {
        canvasCover.SetActive(true);

        PlayerPrefs.SetString("levelToLoad", "Level 1 V2");
        
        yield return SceneManager.LoadSceneAsync("Level 1 V2", LoadSceneMode.Additive);

        yield return SceneManager.LoadSceneAsync("BaseScene", LoadSceneMode.Additive);

        yield return SceneManager.LoadSceneAsync("ProjectileScene", LoadSceneMode.Additive);

        //I think the level that is loaded should be the active scene. But who knows. like Lv1 or Lv2.
        Scene newActiveScene = SceneManager.GetSceneByName("BaseScene");

        SceneManager.SetActiveScene(newActiveScene);

        EventSystem<NewLevelLoadedEvent>.FireEvent(null);
        EventSystem<ActivatePlayerControl>.FireEvent(new ActivatePlayerControl(true));

        //I think this has to happen last. So it doesn't unload the scene while the script is still running.
        yield return SceneManager.UnloadSceneAsync("IntroCutscene");
    }

    private void FirstInput() 
    {
        textInUI.text = "Press Space\nto confirm.";
        firstInputMade = true;
    }

    public void SetLateInIntro(bool newValue) 
    {
        lateInIntro = newValue;
    }

    public void QuitGameFromCutscene()
    {
        Application.Quit();
        Debug.Log("End of ztd cutscene.");
    }
}
