using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public GameVariables gameVariables;
    public static List<GameObject> keyList;

    private void Awake()
    {
        gameManager = this;
    }

    public void OnstartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
