using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ThirdPersonCamera : MonoBehaviour {
    
    [SerializeField] private List<CameraBehaviourPair> cameraBehaviourPairs;
    
    private CameraBehaviour currentCameraBehaviour;
    
    private Dictionary<CameraBehaviourType, CameraBehaviour> cameraBehaviours = new Dictionary<CameraBehaviourType, CameraBehaviour>();
    
    private void Awake() {
        currentCameraBehaviour = cameraBehaviourPairs[0].Behaviour;
        
        currentCameraBehaviour.Init(transform);
        Cursor.ActivateCursor(false, CursorLockMode.Locked);
        
        foreach(CameraBehaviourPair pair in cameraBehaviourPairs)
            cameraBehaviours.Add(pair.CameraType, pair.Behaviour);
        
    }

    private void OnEnable() {
        EventSystem<NewCameraFocus>.RegisterListener(SwitchToFocusBehaviour);
        EventSystem<ResetCameraFocus>.RegisterListener(SwitchToFollowCamera);
    }

    private void OnDisable() {
        EventSystem<NewCameraFocus>.UnregisterListener(SwitchToFocusBehaviour);
        EventSystem<ResetCameraFocus>.UnregisterListener(SwitchToFollowCamera);
    }
    
    void LateUpdate() {
        if(currentCameraBehaviour.enabled)
            currentCameraBehaviour.MovementBehaviour();
    }
    
    private void SwitchToFollowCamera(ResetCameraFocus focus) {
        currentCameraBehaviour = cameraBehaviours[CameraBehaviourType.Follow];
    }
    

    private void SwitchToFocusBehaviour(NewCameraFocus focusEvent) {
        currentCameraBehaviour = cameraBehaviours[CameraBehaviourType.Focus];
        currentCameraBehaviour.Init(focusEvent.Target);
    }
    
}
