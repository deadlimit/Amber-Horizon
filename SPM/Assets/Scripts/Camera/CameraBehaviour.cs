using System;
using UnityEngine;

/// <summary>
/// CameraBehaviour var egentligen en interface men interfaces kan inte serialiseras i inspektorn.
/// </summary>

public enum CameraBehaviourType { Follow, Focus}

[Serializable]
public struct CameraBehaviourPair {
    public CameraBehaviourType CameraType;
    public CameraBehaviour Behaviour;
}

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



