using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbilitySystem;

[CreateAssetMenu(fileName = "HealthRestoreAbility", menuName = "Abilities/HealthRestoreAbility")]
public class HealthRestoreAbility : GameplayAbility
{
    public int healthToRestore { get; set; }
    public override void Activate(GameplayAbilitySystem Owner)
    {
        Owner.ApplyEffectToSelf(AppliedEffect);
    }


}
