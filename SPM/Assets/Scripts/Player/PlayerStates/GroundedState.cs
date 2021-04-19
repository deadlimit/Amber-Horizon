using UnityEngine;

[CreateAssetMenu()]
public class GroundedState : State
{
    PlayerController player;
    protected override void Initialize()
    {
        player = (PlayerController)owner;
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
        //Debug.Log("input från grounded : " + input);
        player.InputGrounded(input);
        Jump();
        Fall();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && player.physics.isGrounded()) {
            player.GetComponent<Animator>().SetTrigger("Jump");
            stateMachine.ChangeState<JumpingState>();
            player.SetJump();
        }
    }
    private void Fall() 
    {
        
        if (player.physics.velocity.y < 0 && !player.physics.isGrounded())
            stateMachine.ChangeState<FallingState>();          
    }
}
