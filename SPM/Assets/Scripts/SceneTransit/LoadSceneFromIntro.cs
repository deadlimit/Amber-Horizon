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
    private bool firstInputMade = false;

    //I don't want the player loading the next level at the same time as the game starts the load.
    //Could cause delay.
    //This value is changed by a signal in the intro cutscene.
    public bool lateInIntro = false;

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
            //StartCoroutine(LoadLevel1());
        }
    }

    public void LoadLevel1Caller() 
    {
        //StartCoroutine(LoadLevel1());
        Debug.Log("End of intro cutscene.");
    }

    private IEnumerator LoadLevel1()
    {
        yield return SceneManager.LoadSceneAsync("Level 1 V2", LoadSceneMode.Additive);

        yield return SceneManager.LoadSceneAsync("BaseScene", LoadSceneMode.Additive);

        yield return SceneManager.LoadSceneAsync("ProjectileScene", LoadSceneMode.Additive);

        EventSystem<NewLevelLoadedEvent>.FireEvent(null);
        EventSystem<ActivatePlayerControl>.FireEvent(new ActivatePlayerControl(true));

        //I think this has to happen last. So it doesn't unload the scene while the script is still running.
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    private void FirstInput() 
    {
        textInUI.text = "Press Space\nto confirm.";
        firstInputMade = true;
    }
}
