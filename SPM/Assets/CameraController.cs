using EventCallbacks;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public void Update() {
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //ToggleLockedState();

        }
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
