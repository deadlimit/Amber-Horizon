using AbilitySystem;
using UnityEngine;
using EventCallbacks;

[CreateAssetMenu(fileName = "FistPunch", menuName = "Abilities/FistPunch")]
public class FistPunch: GameplayAbility {
    
    //Väldigt mycket GetComponent här.
    public override void Activate(GameplayAbilitySystem Owner) {
        Destructor parent = Owner.GetComponent<Destructor>();
        
        GameplayAbilitySystem playerAbilitySystem = parent.GetComponent<GameplayAbilitySystem>();

        Owner.TryApplyEffectToOther(AppliedEffect, playerAbilitySystem);
        
        EventSystem<PlayerHitEvent>.FireEvent(new PlayerHitEvent(Owner.transform, this));
    }
}
