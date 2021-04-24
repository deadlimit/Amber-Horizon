using AbilitySystem;
using UnityEngine;
using EventCallbacks;

[CreateAssetMenu(fileName = "EnemyShoot", menuName = "Abilities/EnemyShoot")]
public class EnemyShoot: GameplayAbility {
    public override void Activate(GameplayAbilitySystem Owner) {
        Enemy parent = Owner.GetComponent<Enemy>();
        GameplayAbilitySystem playerAbilitySystem = parent.GetComponent<GameplayAbilitySystem>();

        Owner.TryApplyEffectToOther(AppliedEffect, playerAbilitySystem);
        
        EventSystem<PlayerHitEvent>.FireEvent(new PlayerHitEvent(Owner.transform, this));
        
    }
}
