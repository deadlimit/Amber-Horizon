using EventCallbacks;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private Animator animator;
    private PhysicsComponent physics;

    private void Awake() {
        animator = GetComponent<Animator>();
        physics = GetComponent<PhysicsComponent>();
    }
    
    private void OnEnable() {
        EventSystem<PlayerHitEvent>.RegisterListener(DestructorHit);
        EventSystem<AbilityUsed>.RegisterListener(PlayDashAnimation);
    }

    private void OnDisable() {
        EventSystem<PlayerHitEvent>.UnregisterListener(DestructorHit);
        EventSystem<AbilityUsed>.UnregisterListener(PlayDashAnimation);

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
        
        //rotera endast y-axel
        transform.LookAt(playerHitEvent.enemyTransform);
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.Euler(rotation);

        float oldMaxSpeed = physics.maxSpeed;
        physics.maxSpeed = 200;
        physics.AddForce(-transform.forward * 10);
        animator.SetTrigger("PunchHit");
        GetComponent<PlayerController>().enabled = false;
        this.Invoke(() => physics.maxSpeed = oldMaxSpeed, 1);
    }

    private void PlayDashAnimation(AbilityUsed ability) {
        if (ability.ability is DashAbility) 
            animator.SetTrigger("Dash");
    }
}
