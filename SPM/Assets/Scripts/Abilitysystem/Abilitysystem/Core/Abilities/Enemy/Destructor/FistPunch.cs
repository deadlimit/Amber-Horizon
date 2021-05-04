using AbilitySystem;
using UnityEngine;
using EventCallbacks;

[CreateAssetMenu(fileName = "FistPunch", menuName = "Abilities/FistPunch")]
public class FistPunch: GameplayAbility {

    public override void Activate(GameplayAbilitySystem Owner) {
        Enemy parent = Owner.GetComponent<Enemy>();
        GameplayAbilitySystem playerAbilitySystem = parent.Target.GetComponent<GameplayAbilitySystem>();
        Owner.TryApplyEffectToOther(AppliedEffect, playerAbilitySystem);
        EventSystem<PlayerHitEvent>.FireEvent(new PlayerHitEvent(Owner.transform, this));
    }
}
