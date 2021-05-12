using System;
using System.Collections.Generic;
using System.Linq;
using EventCallbacks;
using UnityEditor;
using UnityEngine;

/// <summary>
/// CameraBehaviour var egentligen en interface men interfaces kan inte serialiseras i inspektorn
/// </summary>
/// 
public abstract class CameraBehaviour : MonoBehaviour {
    
    protected Transform CameraTransform { get; private set; }
    protected Transform Target;
    public Camera ActiveCamera { get; private set; }
    protected SphereCollider Collider;

    private void Awake() {
        ActiveCamera = Camera.main;
        Collider = GetComponentInParent<SphereCollider>();
        CameraTransform = transform.parent;
    }
    
    public void Init(Transform newTarget) {
        if (newTarget == null) return;
        Target = newTarget;
    }
    
    public abstract void MovementBehaviour();
}

[Serializable]
public struct CameraBehaviourPair {
    public CameraBehaviourType CameraType;
    public CameraBehaviour behaviour;
}

public enum CameraBehaviourType { Follow, Focus}

public class ThirdPersonCamera : MonoBehaviour {
    
    [SerializeField] private List<CameraBehaviourPair> cameraBehaviourPairs;
    
    private CameraBehaviour currentCameraBehaviour;
    private Dictionary<CameraBehaviourType, CameraBehaviour> cameraBehaviours = new Dictionary<CameraBehaviourType, CameraBehaviour>();
    
    private void Awake() {
        
        Cursor.lockState = CursorLockMode.Locked;

        foreach (CameraBehaviourPair pair in cameraBehaviourPairs)
            cameraBehaviours.Add(pair.CameraType, pair.behaviour);


        currentCameraBehaviour = cameraBehaviourPairs.First().behaviour;
        currentCameraBehaviour.Init(transform);

    }

    private void OnEnable() {
        EventSystem<NewCameraFocus>.RegisterListener(SwitchToFocusBehaviour);
        EventSystem<ResetCameraFocus>.RegisterListener(SwitchToFollowCamera);
    }

    private void OnDisable() {
        EventSystem<ResetCameraFocus>.UnregisterListener(SwitchToFollowCamera);
        EventSystem<NewCameraFocus>.UnregisterListener(SwitchToFocusBehaviour);
    }
    
    void LateUpdate() {
        
        currentCameraBehaviour.MovementBehaviour();
        
    }

    private void SwitchToFollowCamera(ResetCameraFocus focus) {
        currentCameraBehaviour = cameraBehaviours[CameraBehaviourType.Follow];
        currentCameraBehaviour.ActiveCamera.orthographic = false;
    }
    
    private void SwitchToFocusBehaviour(NewCameraFocus focusEvent) {
        currentCameraBehaviour = cameraBehaviours[CameraBehaviourType.Focus];
        currentCameraBehaviour.ActiveCamera.orthographic = focusEvent.OrthographicView;
        currentCameraBehaviour.Init(focusEvent.Target);
    }
    
}
