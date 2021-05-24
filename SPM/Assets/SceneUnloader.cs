using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloader : MonoBehaviour {


    [SerializeField] private string sceneToUnload;

    public void OnEnable() {
        StartCoroutine(UnloadScene());
    }

    private IEnumerator UnloadScene() {
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneToUnload));
        
        Debug.Log("Unload Done");
    }


}
