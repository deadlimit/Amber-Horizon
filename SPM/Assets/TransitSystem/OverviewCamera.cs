using Cinemachine;
using EventCallbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OverviewCamera : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera overviewCamera;
    [SerializeField] private GameObject level2OverviewTarget;

    private void OnEnable() {
        EventSystem<NewLevelLoadedEvent>.RegisterListener(MoveCameraToOverviewLocation);
        EventSystem<EnterTransitViewEvent>.RegisterListener(ActivateCamera);
        EventSystem<ResetCameraFocus>.RegisterListener(DeactivateCamera);

        Debug.Log("OverviewCamera. OnEnable");
    }

    private void OnDisable() {
        EventSystem<NewLevelLoadedEvent>.UnregisterListener(MoveCameraToOverviewLocation);
    }

    private void ActivateCamera(EnterTransitViewEvent transitEvent) {
        overviewCamera.Priority = 10;
    }

    private void DeactivateCamera(ResetCameraFocus transitEvent) {
        overviewCamera.Priority = 0;
    }
    
    private void MoveCameraToOverviewLocation(NewLevelLoadedEvent loadedEvent) {
        //overviewCamera.m_Follow = GameObject.FindGameObjectWithTag("TransitOverview").transform;
        if(SceneManager.GetSceneByName("Level 2").isLoaded)
        {
            Debug.Log("OverviewCamera. loading, found Level 2");
            overviewCamera.m_Follow = level2OverviewTarget.transform;
        }
    }
}
