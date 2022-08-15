using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using UnityEngine.SceneManagement;

public class LoadSceneFromIntro : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Debug.Log("Pressed ESC in intro.");
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
}
