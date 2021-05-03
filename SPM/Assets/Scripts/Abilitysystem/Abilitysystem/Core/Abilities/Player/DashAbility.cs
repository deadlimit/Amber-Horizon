using System.Collections;
using AbilitySystem;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Abilities/Dash")]
public class DashAbility : GameplayAbility {

    public float timeWithOutGravity;
    public float dashLength;
    
    public override void Activate(GameplayAbilitySystem Owner) {
        Owner.StartCoroutine(Dash(Owner));
        
    }
    
    private IEnumerator Dash(GameplayAbilitySystem Owner) {
        
        Owner.ApplyEffectToSelf(Cooldown);
        
        PlayerController playerController = Owner.GetComponent<PlayerController>();

        playerController.GetComponent<Animator>().SetTrigger("Dash");

        //Spara gravitationen innan man sätter den till 0
        float gravity = playerController.physics.gravity;

        Vector3 cameraForwardDirection = Camera.main.transform.forward;

        //Nollar y-axeln för att bara dasha framåt.
        cameraForwardDirection.y = 0;

        //Stänger av gravitationen och nollställer hastigheten för att endast dash-velociteten ska gälla. 
        Vector3 forwardMomentum = new Vector3(playerController.physics.velocity.x, 0f, playerController.physics.velocity.z);
        float previousMaxSpeed = playerController.physics.maxSpeed;
        playerController.physics.velocity = Vector3.zero;
        playerController.physics.gravity = 0;
        playerController.physics.maxSpeed = dashLength;
        playerController.force = cameraForwardDirection * dashLength;
        
        //Vänta .4 sekunder innan man sätter på gravitationen igen. 
        yield return new WaitForSeconds(timeWithOutGravity);
        
        playerController.physics.gravity = gravity;
        playerController.force = forwardMomentum;
        playerController.physics.maxSpeed = previousMaxSpeed;
    }
}
