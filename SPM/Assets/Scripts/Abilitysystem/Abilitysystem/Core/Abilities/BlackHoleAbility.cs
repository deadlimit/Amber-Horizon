using AbilitySystem;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackHoleAbility", menuName = "Abilities/BlackHoleAbility")]
public class BlackHoleAbility: GameplayAbility {

    public override void Activate(GameplayAbilitySystem Owner) {
        
        Debug.Log("Fire black hole");
        
        
    }
}
