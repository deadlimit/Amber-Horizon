using EventCallbacks;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {

    private Animator animator;
    private PhysicsComponent physics;
    private void Awake() {
        animator = GetComponent<Animator>();
        physics = GetComponent<PhysicsComponent>();
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
}
