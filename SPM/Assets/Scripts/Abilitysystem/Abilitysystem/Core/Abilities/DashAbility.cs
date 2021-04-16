using System.Collections;
using AbilitySystem;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash", menuName = "Abilities/Dash")]
public class DashAbility : GameplayAbility {

    public float timeWithOutGravity = .4f;
    public float dashLength = 20;
    
    public override void Activate(GameplayAbilitySystem Owner) {
        Owner.StartCoroutine(Dash(Owner));
        
    }
    
    private IEnumerator Dash(GameplayAbilitySystem Owner) {
        
        Owner.ApplyEffectToSelf(Cooldown);
        
        Debug.Log(Cooldown.Duration);
        
        Controller3D controller3D = Owner.GetComponent<Controller3D>();

        controller3D.GetComponent<Animator>().SetTrigger("Dash");

        //Spara gravitationen innan man sätter den till 0
        float gravity = controller3D.playerPhys.gravity;

        Vector3 cameraForwardDirection = Camera.main.transform.forward;

        //Nollar y-axeln för att bara dasha framåt.
        cameraForwardDirection.y = 0;

        //Stänger av gravitationen och nollställer hastigheten för att endast dash-velociteten ska gälla. 
        Vector3 forwardMomentum = new Vector3(controller3D.playerPhys.velocity.x, 0f, controller3D.playerPhys.velocity.z);
        controller3D.playerPhys.velocity = Vector3.zero;
        controller3D.playerPhys.gravity = 0;

        controller3D.velocity = cameraForwardDirection * dashLength;
        controller3D.playerPhys.AddForce(controller3D.velocity);
        
        //Vänta .4 sekunder innan man sätter på gravitationen igen. 
        yield return new WaitForSeconds(timeWithOutGravity);
        
        
        controller3D.playerPhys.gravity = gravity;
        controller3D.velocity = forwardMomentum;
    }
}
