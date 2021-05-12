using AbilitySystem;
using UnityEngine;

[CreateAssetMenu()]
public class JumpingState : State
{
    private PlayerController player;

    protected override void Initialize()
    {
        player = (PlayerController)owner;
    }
    
    public override void RunUpdate() 
    {
        Vector3 input =
        Vector3.right * Input.GetAxisRaw("Horizontal") +
        Vector3.forward * Input.GetAxisRaw("Vertical");
        player.InputAirborne(input, true);
        
        if (player.isGrounded()) 
        {
            stateMachine.ChangeState<GroundedState>();
        }

        if (Input.GetKeyDown(KeyCode.E)) 
        {
            player.abilitySystem.TryActivateAbilityByTag(GameplayTags.MovementAbilityTag);
        }

    }
    

}
