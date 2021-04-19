using System.Collections.Generic;
using AbilitySystem;
using UnityEngine;

public class AbilityEntity : MonoBehaviour {

    public List<GameplayAbility> StartingAbilities;
    public List<GameplayEffect> StartingEffects;
    public List<GameplayAttributeSetEntry> AttributeSet;
    
    private GameplayAbilitySystem abilitySystem;

    private void OnEnable() {
        abilitySystem = gameObject.AddComponent<GameplayAbilitySystem>();
        abilitySystem.RegisterAttributeSet(AttributeSet);
        StartingAbilities.ForEach(a => abilitySystem.GrantAbility(a));
        StartingEffects.ForEach(e => abilitySystem.ApplyEffectToSelf(e));
    }
}
