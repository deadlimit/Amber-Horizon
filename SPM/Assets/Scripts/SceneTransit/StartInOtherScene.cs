using AbilitySystem;
using Cinemachine;
using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartInOtherScene : MonoBehaviour
{
    //Moves the transit camera and/or gives player the dash if not in Level1.

    [SerializeField] private CinemachineVirtualCamera overviewCamera;
    [SerializeField] private GameObject level2OverviewTarget;
    [SerializeField] private GameplayAbility abilityName;
    [SerializeField] private GameObject player;

    //[SerializeField] private Vector3 levelOneStartPosition, levelTwoStartPosition;

    [SerializeField] private bool useStartLocation;
    [SerializeField] private GameObject levelOneStartPosition, levelTwoStartPosition;

    void Start()
    {
        MoveTransitCamera();

        if (useStartLocation) 
        {
            MoveToStartPosition();
        }


        //if the player starts in a scene that isn't level 1 they get the dash.
        if (!SceneManager.GetSceneByName("Level 1 V2").isLoaded) 
        {           
            GiveDashPower();
        }

        //Debug. So I get the dash even in level 1. For testing purposes.
        GiveDashPower();
    }

    private void GiveDashPower() 
    {
        //other.GetComponent<GameplayAbilitySystem>().GrantAbility(ability);
        player.GetComponent<GameplayAbilitySystem>().GrantAbility(abilityName);
        //EventSystem<DisplayUIMessage>.FireEvent(new DisplayUIMessage("New ability: Dash", 4, false));
    }

    private void MoveTransitCamera() 
    {
        //switch case for levels based on level 2.
        if(SceneManager.GetSceneByName("Level 2 V2").isLoaded)
        {
            Debug.Log("StartInOtherScene, MoveTransitCamera");
            overviewCamera.m_Follow = level2OverviewTarget.transform;
        }

    }

    private void MoveToStartPosition() 
    {
        string levelToLoad = PlayerPrefs.GetString("levelToLoad");
        Debug.Log("in StartInOtherScene, Awake. levelToLoad is: " + levelToLoad);

        //Debug. The cases are commented out for testing.
        switch (levelToLoad)
        {
            case "IntroCutscene":
                //do nothing as the player isn't in Intro
                break;
            case "Level 1 V2":
                player.transform.position = levelOneStartPosition.transform.position;
                break;
            case "Level 2 V2":
                player.transform.position = levelTwoStartPosition.transform.position;
                break;
        }
    }
}
