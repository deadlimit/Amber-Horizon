using UnityEngine;

[CreateAssetMenu()]
public class JumpingState : State
{
    Controller3D player;
    protected override void Initialize()
    {
        player = (Controller3D)owner;
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
        player.SetInput(input * 0.2f);

        if (player.playerPhys.velocity.y < 0)
        {

            stateMachine.ChangeState<FallingState>();
        }
        //om vi �r p� v�g ned -> falling state

    }

}
