using AbilitySystem;
using Cinemachine;
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

    void Start()
    {
        if (SceneManager.GetSceneByName("Level 2").isLoaded) 
        {
            GiveDashPower();
            MoveTransitCamera();
        }
    }

    private void GiveDashPower() 
    {
        //other.GetComponent<GameplayAbilitySystem>().GrantAbility(ability);
        player.GetComponent<GameplayAbilitySystem>().GrantAbility(abilityName);
        //EventSystem<DisplayUIMessage>.FireEvent(new DisplayUIMessage("New ability: Dash", 2, true));
    }

    private void MoveTransitCamera() 
    {
        Debug.Log("StartInOtherScene, MoveTransitCamera");
        overviewCamera.m_Follow = level2OverviewTarget.transform;
    }
}
