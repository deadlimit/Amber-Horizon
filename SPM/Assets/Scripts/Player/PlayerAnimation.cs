using EventCallbacks;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private Animator animator;
    private PhysicsComponent physics;
    private void Awake() {
        animator = GetComponent<Animator>();
        physics = GetComponent<PhysicsComponent>();
<<<<<<< Updated upstream

    }

    private void OnEnable() {
        EventSystem<PlayerHitEvent>.RegisterListener(DestructorHit);
        EventSystem<AbilityUsed>.RegisterListener(PlayDashAnimation);
    }

    private void OnDisable() {
        EventSystem<PlayerHitEvent>.UnregisterListener(DestructorHit);
        EventSystem<AbilityUsed>.UnregisterListener(PlayDashAnimation);
=======
>>>>>>> Stashed changes
    }


    private void OnEnable() {
        EventSystem<AbilityUsed>.RegisterListener(DashAnimation);
        EventSystem<PlayerHitEvent>.RegisterListener(DestructorHit);
    }

    private void Update() {

        float zAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");
        animator.SetFloat("VelocityX", xAxis);
        animator.SetFloat("VelocityZ", zAxis);

        if (Input.GetKeyDown(KeyCode.Tab))
            animator.SetBool("ShowKey", !animator.GetBool("ShowKey"));
    }

    //Används i en animationstrigger
    public void ReturnPlayerControl() {
        GetComponent<PlayerController>().enabled = true;
    }

    private void DestructorHit(PlayerHitEvent playerHitEvent) {
        if (!(playerHitEvent.ability is FistPunch)) return;
        
        transform.LookAt(playerHitEvent.enemyTransform);
        float oldMaxSpeed = physics.maxSpeed;

        physics.maxSpeed = 200;
        physics.AddForce(-transform.forward * 10);
        animator.SetTrigger("PunchHit");
        GetComponent<PlayerController>().enabled = false;
        this.Invoke(() => physics.maxSpeed = oldMaxSpeed, 1);
    }

<<<<<<< Updated upstream
    private void PlayDashAnimation(AbilityUsed ability) {
        print("fire");
        print(ability.ability);
        if (ability.ability is AirDashAbility) {
            animator.SetTrigger("Dash");
            print("yeah");
        }
=======
    private void DashAnimation(AbilityUsed abilityUsed) {
        
        print("was not dash");
        if (abilityUsed.ability is DashAbility) {
            print("was dash");
            animator.SetTrigger("Dash");
        }
        
>>>>>>> Stashed changes
            
    }
}
