using AbilitySystem;
using UnityEngine;
using EventCallbacks;

[CreateAssetMenu(fileName = "FistPunch", menuName = "Abilities/FistPunch")]
public class FistPunch: GameplayAbility {

    //The functionality of this ability is located in the DestructorAttackZone-script, together with an explanation of why.
    public override void Activate(GameplayAbilitySystem Owner) {}
}
