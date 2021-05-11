using System.Collections;
using System.Runtime.CompilerServices;
using AbilitySystem;
using EventCallbacks;
using UnityEngine;

[CreateAssetMenu(fileName = "Air Dash", menuName = "Abilities/Air Dash")]
public class DashAbility : GameplayAbility {

    [SerializeField] private float timeWithOutGravity;
    [SerializeField] private float dashLength;
    
    public override void Activate(GameplayAbilitySystem Owner) {
        Owner.StartCoroutine(Dash(Owner));
    }
    
    private IEnumerator Dash(GameplayAbilitySystem Owner) {
        PlayerController playerController = Owner.GetComponent<PlayerController>();
        
        
        //Spara gravitationen innan man sätter den till 0
        
        Vector3 cameraForwardDirection = Camera.main.transform.forward;

        //Nollar y-axeln för att bara dasha framåt.
        cameraForwardDirection.y = 0;

        //Stänger av gravitationen och nollställer hastigheten för att endast dash-velociteten ska gälla. 
        Vector3 forwardMomentum = new Vector3(playerController.physics.velocity.x, 0f, playerController.physics.velocity.z);
        float previousMaxSpeed = playerController.physics.maxSpeed; 
        float gravity = playerController.physics.gravity;
        
        playerController.physics.velocity = Vector3.zero;
        playerController.physics.gravity = playerController.isGrounded() ? gravity * 3 : 0;
        playerController.physics.maxSpeed = dashLength;

        //förlåt för divison med DT, det är hemskt och beror på hur fixen med FPS-problemen är. Ska göra om allt senare.. om jag hinner.
        playerController.physics.AddForce(cameraForwardDirection * dashLength / Time.deltaTime);
        
        //Vänta .4 sekunder innan man sätter på gravitationen igen. 
        yield return new WaitForSeconds(timeWithOutGravity);
        
        playerController.physics.gravity = gravity;
        playerController.force = forwardMomentum;
        playerController.physics.maxSpeed = previousMaxSpeed;
    }
}
