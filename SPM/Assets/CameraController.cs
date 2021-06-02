using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private bool canMove;
    private Animator animator;
    private PlayerController playerController;
    private ThirdPersonCamera cameraController;
    [SerializeField] private CinemachineVirtualCamera playerCamera, keyLookAtCamera;
    
    private static readonly int ShowKey = Animator.StringToHash("ShowKey");

    private void Awake() {
        canMove = false;
        playerController = FindObjectOfType<PlayerController>();
        cameraController = FindObjectOfType<ThirdPersonCamera>();
        animator = playerController.GetComponent<Animator>();
    }
    public void Update() {
        
        if (Input.GetKeyDown(KeyCode.Tab)) {
                ToggleLockedState(canMove);
        }
    }

    private void ToggleLockedState(bool value) {
        animator.SetBool(ShowKey, !animator.GetBool(ShowKey));
        
        playerController.enabled = value;
        cameraController.enabled = value;
        canMove = !canMove;
    }
    
    
    /*
    private void ToggleLockedState()
    {   //Could be placed inside state but i wanted to gather all the inputs, also considered calling an overridden method inside the states,
        //but that would be bloat for all other uses of the state machine core
        if (stateMachine.CurrentState.GetType() == typeof(GroundedState))
        {
            stateMachine.ChangeState<PlayerLockedState>();
        }
        else if (stateMachine.CurrentState.GetType() == typeof(PlayerLockedState))
        {
            EventSystem<ResetCameraFocus>.FireEvent(null);
            stateMachine.ChangeState<GroundedState>();
        }

        animator.SetBool("ShowKey", !animator.GetBool("ShowKey"));
    }
    */

}
