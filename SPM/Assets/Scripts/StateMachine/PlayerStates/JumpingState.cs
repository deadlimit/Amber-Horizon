using System.Collections;
using System.Collections.Generic;
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

        /*  if (player.GetPhysics().isGrounded())
          {
              //jag tror att det som händer är att den hinner utvärdera isGrounded() innan vi helt lämnat marken, därför 
              //går den direkt tillbaka till GroundedState, men INTE om man gör dubbelhoppet som blir möjligt av den här buggen
              stateMachine.ChangeState<GroundedState>();
          }*/


        if (player.GetPhysics().GetVelocity().y < 0)
        {

            stateMachine.ChangeState<FallingState>();
        }
        //om vi är på väg ned -> falling state

    }

}
