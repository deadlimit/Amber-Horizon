using AbilitySystem;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackHoleAbility", menuName = "Abilities/BlackHoleAbility")]
public class BlackHoleAbility: GameplayAbility {
    

    public BlackHole bh;

    public override void Activate(GameplayAbilitySystem Owner) 
    {
        AimingAbility aa = (AimingAbility)Owner.GetAbilityByTag(typeof(AimingTag));
        Debug.Assert(aa);
        aa.FireBlackHole();
    }
}
