using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public GameVariables gameVariables;
    public static List<GameObject> keyList;

    private void Awake()
    {
        gameManager = this;
    }
   
}
