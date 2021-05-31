using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbilitySystem;
using EventCallbacks;

public class DestructorAttackZone : MonoBehaviour
{
    private Destructor owner;
    private GameplayAbility ability;
    private void Awake()
    {
        
        owner = GetComponentInParent<Destructor>();
        ability = owner.AbilitySystem.GetAbilityByTag(GameplayTags.MeleeTag);
    }

    //Really dumb solution to performing an ability on a target abilitysystem, which is not possible to do outside
    //the actual abilities inside the GAS, since ApplyEffectToSelf takes a gameplay effect, and that is not available without first fetching the ability
    //by tag, and then fetching the ability's applied effect. So at this point, for the destructor, the ability system is only for checking if applying a certain ability is legal 
    //due to cooldown or possibly blockedBy-tags.
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("AttackZone hit player");

            other.GetComponent<GameplayAbilitySystem>().ApplyEffectToSelf(ability.AppliedEffect);
            EventSystem<PlayerHitEvent>.FireEvent(new PlayerHitEvent(owner.transform, ability.AppliedEffect));
            EventSystem<SoundEffectEvent>.FireEvent(new SoundEffectEvent(ability.soundEffect));
            EventSystem<EnterSlowMotionEvent>.FireEvent(new EnterSlowMotionEvent(2));

        }
    }
}
