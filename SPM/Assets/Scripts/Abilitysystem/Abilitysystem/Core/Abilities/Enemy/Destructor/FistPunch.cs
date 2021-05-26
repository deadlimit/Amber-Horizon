using AbilitySystem;
using UnityEngine;
using EventCallbacks;

[CreateAssetMenu(fileName = "FistPunch", menuName = "Abilities/FistPunch")]
public class FistPunch: GameplayAbility {

    public AudioClip soundEffect;
    
    public override void Activate(GameplayAbilitySystem Owner) {
        Enemy parent = Owner.GetComponent<Enemy>();
        GameplayAbilitySystem playerAbilitySystem = parent.Target.GetComponent<GameplayAbilitySystem>();
        
        if (Owner.TryApplyEffectToOther(AppliedEffect, playerAbilitySystem))
        {
            EventSystem<PlayerHitEvent>.FireEvent(new PlayerHitEvent(Owner.transform, AppliedEffect, playerAbilitySystem.gameObject.GetComponent<PlayerController>()));
            EventSystem<SoundEffectEvent>.FireEvent(new SoundEffectEvent(soundEffect));
            EventSystem<EnterSlowMotionEvent>.FireEvent(new EnterSlowMotionEvent(2));
        }
    }
}
