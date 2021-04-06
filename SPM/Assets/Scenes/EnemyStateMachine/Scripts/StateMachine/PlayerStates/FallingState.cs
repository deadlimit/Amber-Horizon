using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class FallingState : State
{
    Controller3D player;
    protected override void Initialize()
    {
        player = (Controller3D)owner;
    }

    public override void Enter() {
        //Debug.Log("Falling State!"); 
    }
    public override void RunUpdate()
    {
        Vector3 input =
        Vector3.right * Input.GetAxisRaw("Horizontal") +
        Vector3.forward * Input.GetAxisRaw("Vertical");
        input = input.normalized;
        player.SetInput(input * 0.2f);
        player.GetPhysics().AddFallSpeed();
        //hur fasen ändrar man hastigheten man faller med.. om man bara gör input.y så kommer det ju med i rotationen.
        //ev. kalla ny metod i spelaren/fysiken som påverkar velocity.

        if (player.GetPhysics().isGrounded())
        {
            //jag tror att det som händer är att den hinner utvärdera isGrounded() innan vi helt lämnat marken, därför 
            //går den direkt tillbaka till GroundedState, men INTE om man gör dubbelhoppet som blir möjligt av den här buggen
            stateMachine.ChangeState<GroundedState>();
        }
        
        //om vi är på väg ned -> falling state

    }
}
