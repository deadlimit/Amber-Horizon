using Cinemachine;
using EventCallbacks;
using UnityEngine;

public class OverviewCamera : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera overviewCamera;
    
    private void OnEnable() {
        EventSystem<NewLevelLoadedEvent>.RegisterListener(MoveCameraToOverviewLocation);
        EventSystem<EnterTransitViewEvent>.RegisterListener(ActivateCamera);
        EventSystem<ResetCameraFocus>.RegisterListener(DeactivateCamera);
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
        overviewCamera.m_Follow = GameObject.FindGameObjectWithTag("TransitOverview").transform;
    }
}
