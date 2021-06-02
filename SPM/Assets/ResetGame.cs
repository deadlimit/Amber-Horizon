using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetGame : MonoBehaviour {


    public void Reset() {
        StartCoroutine(UnloadScenes());
    }

    private IEnumerator UnloadScenes() {
        yield return SceneManager.UnloadSceneAsync("ProjectileScene");
        yield return SceneManager.UnloadSceneAsync("BaseScene");

        yield return SceneManager.LoadSceneAsync("MainMenu");
        yield return SceneManager.UnloadSceneAsync("Level 2");
    }

}
