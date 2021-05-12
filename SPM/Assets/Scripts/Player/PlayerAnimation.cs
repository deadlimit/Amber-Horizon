using System.Collections.Generic;
using AbilitySystem;
using EventCallbacks;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimation : MonoBehaviour {

    private Animator animator; 
    private PhysicsComponent physics;

    //Lista i inspektorn så man kan tilldela animationscallbacks till PlayerHitEvent-effekter.
    public List<AnimationEffectPair> effectCallbackPairs;

    private Dictionary<GameplayEffect, UnityEvent<Transform>> hitAnimationCallbacks = new Dictionary<GameplayEffect, UnityEvent<Transform>>();
    
    private void Awake() {
        animator = GetComponent<Animator>();
        physics = GetComponent<PhysicsComponent>();

        foreach (AnimationEffectPair pair in effectCallbackPairs)
            hitAnimationCallbacks.Add(pair.Effect, pair.Callback);

    }
    
    private void OnEnable() {
        EventSystem<PlayerHitEvent>.RegisterListener(OnPlayerHit);
        EventSystem<AbilityUsed>.RegisterListener(PlayDashAnimation);
        EventSystem<PlayerDiedEvent>.RegisterListener(OnPlayerDied);
    }

    private void OnDisable() {
        EventSystem<PlayerHitEvent>.UnregisterListener(OnPlayerHit);
        EventSystem<AbilityUsed>.UnregisterListener(PlayDashAnimation);
        EventSystem<PlayerDiedEvent>.UnregisterListener(OnPlayerDied);

    }

    private void Update() {
        float zAxis = Input.GetAxis("Vertical");
        float xAxis = Input.GetAxis("Horizontal");
        animator.SetFloat("VelocityX", xAxis);
        animator.SetFloat("VelocityZ", zAxis);
    }

    //Används i en animationstrigger
    public void ReturnPlayerControl() {
        GetComponent<PlayerController>().enabled = true;
    }

    private void OnPlayerHit(PlayerHitEvent playerHitEvent) {
        if (hitAnimationCallbacks.ContainsKey(playerHitEvent.appliedEffect) == false) return;
            
        hitAnimationCallbacks[playerHitEvent.appliedEffect].Invoke(playerHitEvent.culprit);
    }

    public void OnDestructorHit(Transform culprit) {

        transform.LookAt(culprit);
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

    private void OnPlayerDied(PlayerDiedEvent playerDiedEvent) {
        animator.SetTrigger("PlayerDeath");
    }

    private void OnDeathAnimationDone() {
        EventSystem<PlayerReviveEvent>.FireEvent(null);
        animator.SetTrigger("PlayerRevive");
    }

    public void OnForagerHit(Transform culprit) {
        print("forager hit you");
    }

    private void PlayDashAnimation(AbilityUsed ability) {
        if (ability.ability is DashAbility) 
            animator.SetTrigger("Dash");
    }
}
