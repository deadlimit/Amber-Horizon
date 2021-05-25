using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    
    [MenuItem("Load scenes/Load Main Menu")]
    private static void LoadMainMenu() {
        UnloadCurrentScenes();
        LoadScene("MainMenu");
        LoadScene("ProjectileScene");
    }
   
    [MenuItem("Load scenes/Load Level 1")]
    private static void LoadLevel1() {
        UnloadCurrentScenes();
        LoadScene("BaseScene");
        LoadScene("Level 1");
        LoadScene("ProjectileScene");
    }

    private static void LoadScene(string sceneName) {
        EditorSceneManager.OpenScene("Assets/Scenes/ActiveScenes/" + sceneName + ".unity", OpenSceneMode.Additive);
    }

    private static void UnloadCurrentScenes() {
        for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
            EditorSceneManager.CloseScene(EditorSceneManager.GetSceneAt(i), true);
    }


}
