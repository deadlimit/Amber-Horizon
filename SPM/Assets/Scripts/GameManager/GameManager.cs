using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public GameVariables gameVariables;

    private void Awake()
    {
        gameManager = this;
    }
   
}
