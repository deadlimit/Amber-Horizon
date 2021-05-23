using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Extensions {

    public static void InstantiateObjectInAdditiveScene(this PoolObject poolObject, Scene scene) {
        SceneManager.MoveGameObjectToScene(poolObject.gameObject, scene);
    }
    
    public static void InstantiateObjectsInAdditiveScene(this List<GameObject> gameObjects, string sceneName) {
        foreach(GameObject gameObject in gameObjects)
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(sceneName));
    }
    
}
