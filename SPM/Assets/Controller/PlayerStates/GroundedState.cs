using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GroundedState : State
{
    Controller3D player;
    PhysicsComponent pp;
    protected override void Initialize()
    {
        player = (Controller3D)owner;
        pp = player.GetPhysics();
        Debug.Assert(player);
    }

    public override void Enter()
    {
        //Debug.Log("Grounded State!");
    }
    public override void RunUpdate()
    {
        //Debug.Log("Grounded");
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
        if (Input.GetKeyDown(KeyCode.Space) && pp.isGrounded())
        {
            stateMachine.ChangeState<JumpingState>();
            player.SetJump();
        }
    }
    private void Fall() 
    {
        
        if (pp.GetVelocity().y < 0 && !pp.isGrounded())
            stateMachine.ChangeState<FallingState>();          
    }
}
