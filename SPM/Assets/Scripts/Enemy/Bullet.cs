using System;
using AbilitySystem;
using EventCallbacks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour {
    
    public float bulletSpeed;
    private Vector3 direction;
    private Rigidbody activeRigidbody;
    private Forager parent;
    private GameplayAbility ability;
    
    public void Init(GameplayAbility ability, Forager parent) {
        this.ability = ability;
        this.parent = parent;
        activeRigidbody = GetComponent<Rigidbody>();
        direction = parent.Target.transform.position - transform.position;

        Destroy(gameObject, 3f);
    }

    private void Update() {
        activeRigidbody.AddForce(direction.normalized * bulletSpeed);
    }
    
    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            GameplayAbilitySystem playerAbilitySystem = other.gameObject.GetComponent<GameplayAbilitySystem>();
            parent.AbilitySystem.TryApplyEffectToOther(ability.AppliedEffect, playerAbilitySystem);

            EventSystem<PlayerHitEvent>.FireEvent(new PlayerHitEvent(transform, ability));
        }
        Destroy(gameObject);
    }
    
    
    
}
