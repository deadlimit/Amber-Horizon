using AbilitySystem;
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
        
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            player.abilitySystem.TryActivateAbilityByTag(GameplayTags.MovementAbilityTag);
        }
        
        if (player.isGrounded()) 
        {
            //jag tror att det som händer är att den hinner utvärdera isGrounded() innan vi helt lämnat marken, därför 
            //går den direkt tillbaka till GroundedState, men INTE om man gör dubbelhoppet som blir möjligt av den här buggen
            stateMachine.ChangeState<GroundedState>();
        }

    }

}
