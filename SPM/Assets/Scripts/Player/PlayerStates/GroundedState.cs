using UnityEngine;

[CreateAssetMenu()]
public class GroundedState : State
{
    Controller3D player;
    protected override void Initialize()
    {
        player = (Controller3D)owner;
        Debug.Assert(player);
    }

    public override void Enter()
    {
        //Debug.Log("Grounded State!");
    }
    public override void RunUpdate()
    {
        Vector3 input =
        Vector3.right * Input.GetAxisRaw("Horizontal") +
        Vector3.forward * Input.GetAxisRaw("Vertical");
        input = input.normalized;
        player.SetInput(input);
        Jump();
        Fall();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && player.playerPhys.isGrounded()) {
            player.GetComponent<Animator>().SetTrigger("Jump");
            stateMachine.ChangeState<JumpingState>();
            player.SetJump();
        }
    }
    private void Fall() 
    {
        
        if (player.playerPhys.velocity.y < 0 && !player.playerPhys.isGrounded())
            stateMachine.ChangeState<FallingState>();          
    }
}
