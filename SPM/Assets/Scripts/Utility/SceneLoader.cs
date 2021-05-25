using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    private static Scene previousActiveScene;
   
    [MenuItem("Load scenes/Load Main Menu")]
    private static void LoadMainMenu() {
        UnloadCurrentScenes();
        SetNewActiveScene(LoadScene("MainMenu"));
        LoadScene("ProjectileScene");
        UnloadPreviousActiveScene();
    }
   
    [MenuItem("Load scenes/Load Level 1")]
    private static void LoadLevel1() {
        UnloadCurrentScenes();
        SetNewActiveScene(LoadScene("BaseScene"));
        LoadScene("Level 1");
        LoadScene("ProjectileScene");
        UnloadPreviousActiveScene();
    }

    [MenuItem("Load scenes/Load Level 2")]
    private static void LoadLevel2() {
        UnloadCurrentScenes();
        SetNewActiveScene(LoadScene("BaseScene"));
        LoadScene("Level 2");
        LoadScene("ProjectileScene");
        UnloadPreviousActiveScene();
    }

    
    private static Scene LoadScene(string sceneName) {
        return EditorSceneManager.OpenScene("Assets/Scenes/ActiveScenes/" + sceneName + ".unity", OpenSceneMode.Additive);
    }

    private static void UnloadCurrentScenes() {
        previousActiveScene = EditorSceneManager.GetActiveScene();
        
        for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++) {
            
            Scene currentScene = EditorSceneManager.GetSceneAt(i);
            
            if(currentScene == previousActiveScene)
                continue;
            
            EditorSceneManager.CloseScene(currentScene, true);
        }

    }

    private static void UnloadPreviousActiveScene() {
        EditorSceneManager.CloseScene(previousActiveScene, true);
    }
    private static void SetNewActiveScene(Scene scene) {
        EditorSceneManager.SetActiveScene(scene);
    }

}
