using System.Collections.Generic;
using AbilitySystem;
using UnityEngine;

public class AbilityEntity : MonoBehaviour {

    public List<GameplayAbility> StartingAbilities;
    public List<GameplayEffect> StartingEffects;
    public List<GameplayAttributeSetEntry> AttributeSet;
    
    private GameplayAbilitySystem abilitySystem;

    //Ändrade denna från OnEnable till Awake, verkar fungera utan problem? 
    //behöver ha tillgång till det instansierade GameplayAbilitySystem:et i 
    //PlayerHitListener (som nu ligger som eget script på player);
    private void Awake() {
        abilitySystem = gameObject.AddComponent<GameplayAbilitySystem>();
        abilitySystem.RegisterAttributeSet(AttributeSet);
        StartingAbilities.ForEach(gameplayAbility => abilitySystem.GrantAbility(gameplayAbility));
        StartingEffects.ForEach(gameplayEffect => abilitySystem.ApplyEffectToSelf(gameplayEffect));
    }

}
