using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameVariables gameVariables;
    public void OnstartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
