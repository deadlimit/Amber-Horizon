using AbilitySystem;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackHoleAbility", menuName = "Abilities/BlackHoleAbility")]
public class BlackHoleAbility: GameplayAbility {
    

    public BlackHole bh;

    public override void Activate(GameplayAbilitySystem Owner) 
    {
        AbilityEntity ae = Owner.GetComponent<AbilityEntity>();
        AimingAbility aa = (AimingAbility) ae.StartingAbilities[1];
        Debug.Assert(aa);
        aa.FireBlackHole();
    }
}
