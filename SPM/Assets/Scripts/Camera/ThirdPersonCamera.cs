using System;
using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;

/// <summary>
/// CameraBehaviour var egentligen en interface men interfaces kan inte serialiseras i inspektorn
/// </summary>
/// 
public abstract class CameraBehaviour : MonoBehaviour {
    
    protected Transform CameraTransform { get; private set; }
    protected Transform Target;
    protected Camera ActiveCamera;
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
    [SerializeField] private CameraBehaviour currentCameraBehaviour;
    
    private Dictionary<CameraBehaviourType, CameraBehaviour> cameraBehaviours = new Dictionary<CameraBehaviourType, CameraBehaviour>();
    
    private void Awake() {
        currentCameraBehaviour.Init(transform);
        Cursor.lockState = CursorLockMode.Locked;
        
        foreach(CameraBehaviourPair pair in cameraBehaviourPairs)
            cameraBehaviours.Add(pair.CameraType, pair.behaviour);
        
    }

    private void OnEnable() {
        EventSystem<NewCameraFocus>.RegisterListener(SwitchToFocusBehaviour);
    }

    private void OnDisable() {
        EventSystem<NewCameraFocus>.UnregisterListener(SwitchToFocusBehaviour);
    }
    
    void LateUpdate() {
        
        currentCameraBehaviour.MovementBehaviour();
        
    }

    private void SwitchToFocusBehaviour(NewCameraFocus focusEvent) {
        currentCameraBehaviour = cameraBehaviours[CameraBehaviourType.Focus];
        currentCameraBehaviour.Init(focusEvent.Target);
    }
    
}
