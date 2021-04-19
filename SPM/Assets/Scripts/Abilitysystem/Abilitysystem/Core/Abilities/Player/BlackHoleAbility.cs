using AbilitySystem;
using UnityEngine;

[CreateAssetMenu(fileName = "BlackHoleAbility", menuName = "Abilities/BlackHoleAbility")]
public class BlackHoleAbility: GameplayAbility {
    

    public BlackHole bh;

    public override void Activate(GameplayAbilitySystem Owner) {


          GameObject launchPoint = GameObject.FindGameObjectWithTag("LaunchPoint");
          BlackHole obj = Instantiate(bh, launchPoint.gameObject.transform.position, Quaternion.identity);

        //USCH
          obj.velocity = Owner.gameObject.GetComponent<PlayerController>().bhVelocity ;

    }

}
