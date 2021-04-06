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
        //hur fasen �ndrar man hastigheten man faller med.. om man bara g�r input.y s� kommer det ju med i rotationen.
        //ev. kalla ny metod i spelaren/fysiken som p�verkar velocity.

        if (player.GetPhysics().isGrounded())
        {
            //jag tror att det som h�nder �r att den hinner utv�rdera isGrounded() innan vi helt l�mnat marken, d�rf�r 
            //g�r den direkt tillbaka till GroundedState, men INTE om man g�r dubbelhoppet som blir m�jligt av den h�r buggen
            stateMachine.ChangeState<GroundedState>();
        }
        
        //om vi �r p� v�g ned -> falling state

    }
}
