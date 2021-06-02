using System.Collections.Generic;
using AbilitySystem;
using EventCallbacks;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimation : MonoBehaviour {

    private Animator animator; 
    private PhysicsComponent physics;
    private PlayerController playerController;

    //Lista i inspektorn så man kan tilldela animationscallbacks till PlayerHitEvent-effekter.
    public List<AnimationEffectPair> effectCallbackPairs;

    private Dictionary<GameplayEffect, UnityEvent<Transform>> hitAnimationCallbacks = new Dictionary<GameplayEffect, UnityEvent<Transform>>();
    
    private void Awake() {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        physics = GetComponent<PhysicsComponent>();

        foreach (AnimationEffectPair pair in effectCallbackPairs)
            hitAnimationCallbacks.Add(pair.Effect, pair.Callback);

    }
    
    private void OnEnable() {
        EventSystem<StartHitAnimationEvent>.RegisterListener(StartHitAnimation);
        EventSystem<AbilityUsed>.RegisterListener(PlayDashAnimation);
        EventSystem<PlayerDiedEvent>.RegisterListener(OnPlayerDied);
    }

    private void OnDisable() {
        EventSystem<StartHitAnimationEvent>.UnregisterListener(StartHitAnimation);
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
        playerController.enabled = true;
        //physics.maxSpeed = oldMaxSpeed;
    }

    //is "Culprit" needed here? Shouldnt we be able to populate the referenced dictionary by attack types? 
    private void StartHitAnimation(StartHitAnimationEvent startHitAnimationEvent) {
        //Debug.Log(startHitAnimationEvent.appliedEffect);
        if (hitAnimationCallbacks.ContainsKey(startHitAnimationEvent.appliedEffect) == false) return;
        
        hitAnimationCallbacks[startHitAnimationEvent.appliedEffect].Invoke(startHitAnimationEvent.culprit);
    }

    public void OnDestructorHit(Transform culprit) {

       

        transform.LookAt(culprit);
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        transform.rotation = Quaternion.Euler(rotation);

        //Only to be used if player position is actually moved when hit by destructor, right now its not so its commented out
        /*oldMaxSpeed = physics.maxSpeed;
        physics.maxSpeed = 200;*/


        physics.AddForce(-transform.forward * 10);
        animator.SetTrigger("PunchHit");
        playerController.enabled = false;
        playerController.DeactivateAim();

    }

    private void OnPlayerDied(PlayerDiedEvent playerDiedEvent) {
        playerController.enabled = false;
        animator.SetTrigger("PlayerDeath");
        playerController.DeactivateAim();
    }

    //Called by AnimationEvent "PlayerDeath"
    private void OnDeathAnimationDone() {
        Debug.Log("OnDeathAnimationDone Called");
        PlayerReviveEvent pre = new PlayerReviveEvent(gameObject);
        EventSystem<PlayerReviveEvent>.FireEvent(pre);
        animator.SetTrigger("PlayerRevive");
        ReturnPlayerControl();
    }

    public void OnForagerHit(Transform culprit) {
        //print("forager hit you");
    }

    private void PlayDashAnimation(AbilityUsed ability) {
        if (ability.ability is DashAbility) 
            animator.SetTrigger("Dash");
    }
}
