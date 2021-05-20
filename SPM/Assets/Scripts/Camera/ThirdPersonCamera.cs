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
        print(SceneManager.GetSceneByName("MainMenu").isLoaded);
        
        if (SceneManager.GetSceneByName("MainMenu").isLoaded) {
            DisableCamera(null);
        }
        else
            Cursor.lockState = CursorLockMode.Locked;
            
        currentCameraBehaviour.Init(transform);
        Cursor.lockState = CursorLockMode.Locked;
        
        foreach(CameraBehaviourPair pair in cameraBehaviourPairs)
            cameraBehaviours.Add(pair.CameraType, pair.Behaviour);
        
    }

    private void OnEnable() {
        EventSystem<NewCameraFocus>.RegisterListener(SwitchToFocusBehaviour);
        EventSystem<ResetCameraFocus>.RegisterListener(SwitchToFollowCamera);
        EventSystem<LoadMainMenu>.RegisterListener(DisableCamera);
        EventSystem<ExitMainMenu>.RegisterListener(EnableCamera);
        
        
    }

    private void OnDisable() {
        EventSystem<NewCameraFocus>.UnregisterListener(SwitchToFocusBehaviour);
        EventSystem<ResetCameraFocus>.UnregisterListener(SwitchToFollowCamera);
        EventSystem<LoadMainMenu>.UnregisterListener(DisableCamera);
        EventSystem<ExitMainMenu>.UnregisterListener(EnableCamera);
    }
    
    void LateUpdate() {
        if(currentCameraBehaviour.enabled)
            currentCameraBehaviour.MovementBehaviour();
    }

    private void DisableCamera(LoadMainMenu menuLoad) {
        currentCameraBehaviour.enabled = false;
        print("false");
    }

    private void EnableCamera(ExitMainMenu exit) {
        currentCameraBehaviour.enabled = true;
        print("true ");

    }
    
    private void SwitchToFollowCamera(ResetCameraFocus focus) {
        currentCameraBehaviour = cameraBehaviours[CameraBehaviourType.Follow];
    }
    

    private void SwitchToFocusBehaviour(NewCameraFocus focusEvent) {
        currentCameraBehaviour = cameraBehaviours[CameraBehaviourType.Focus];
        currentCameraBehaviour.Init(focusEvent.Target);
    }
    
}
