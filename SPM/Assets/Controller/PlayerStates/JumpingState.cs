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
              //jag tror att det som h�nder �r att den hinner utv�rdera isGrounded() innan vi helt l�mnat marken, d�rf�r 
              //g�r den direkt tillbaka till GroundedState, men INTE om man g�r dubbelhoppet som blir m�jligt av den h�r buggen
              stateMachine.ChangeState<GroundedState>();
          }*/


        if (player.playerPhys.velocity.y < 0)
        {

            stateMachine.ChangeState<FallingState>();
        }
        //om vi �r p� v�g ned -> falling state

    }

}
