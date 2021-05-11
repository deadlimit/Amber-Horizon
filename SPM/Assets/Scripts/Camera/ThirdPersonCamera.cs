using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {
    
    private StateMachine stateMachine;
    [SerializeField] private State[] states;
   
    void Awake() {
        stateMachine = new StateMachine(this, states);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate() {
        stateMachine.RunUpdate();
    }
    
}
