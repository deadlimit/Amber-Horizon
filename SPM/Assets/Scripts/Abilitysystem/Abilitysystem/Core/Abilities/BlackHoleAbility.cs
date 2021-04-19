using AbilitySystem;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackHoleAbility", menuName = "Abilities/BlackHoleAbility")]
public class BlackHoleAbility: GameplayAbility {
    

    public BlackHole bh;



    public override void Activate(GameplayAbilitySystem Owner) {
        
          BlackHole obj = Instantiate(bh, Owner.gameObject.transform.position, Quaternion.identity);
         // obj.velocity = vo;

    }

}
