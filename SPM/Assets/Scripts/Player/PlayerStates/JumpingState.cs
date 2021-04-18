using UnityEngine;

[CreateAssetMenu()]
public class JumpingState : State
{
    PlayerController player;
    protected override void Initialize()
    {
        player = (PlayerController)owner;
    }

    public override void Enter() {
        //Debug.Log("Jumping State!"); 
    }
    public override void RunUpdate() 
    {
        Vector3 input =
        Vector3.right * Input.GetAxisRaw("Horizontal") +
        Vector3.forward * Input.GetAxisRaw("Vertical");
        input = input.normalized;
        player.InputAirborne(input, true);

        if (player.physics.velocity.y < 0)
        {
            stateMachine.ChangeState<FallingState>();
        }
        //om vi �r p� v�g ned -> falling state

    }

}
