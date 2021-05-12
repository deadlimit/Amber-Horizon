using System.Collections.Generic;
using UnityEngine;

public class CharacterController3D : MonoBehaviour {
    
    private StateMachine stateMachine;
    
    [HideInInspector] public Transform ActiveCamera;
    [HideInInspector] public CustomPhysics3D Physics3D;
    public State [] States;
    public float GroundCheckDistance;
    
    
    private void Awake() {
        stateMachine = new StateMachine(this, States);
        ActiveCamera = Camera.main.transform;
        Physics3D = GetComponent<CustomPhysics3D>();
    } 

    private void LateUpdate() => stateMachine.RunUpdate();
    
}
