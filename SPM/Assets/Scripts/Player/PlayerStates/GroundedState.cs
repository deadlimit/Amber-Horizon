using UnityEngine;

[CreateAssetMenu()]
public class GroundedState : State
{
    private PlayerController player;
    protected override void Initialize()
    {
        player = (PlayerController)owner;
        Debug.Assert(player);
    }
    
    public override void RunUpdate()
    {
        if(!player.isGrounded())
            stateMachine.ChangeState<JumpingState>();

            Vector3 input =
        Vector3.right * Input.GetAxisRaw("Horizontal") +
        Vector3.forward * Input.GetAxisRaw("Vertical");
        input = input.normalized;

        player.InputGrounded(input);
        Jump();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && player.isGrounded()) {
            player.GetComponent<Animator>().SetTrigger("Jump");
            stateMachine.ChangeState<JumpingState>();
            player.SetJump();
        }
    }
}
