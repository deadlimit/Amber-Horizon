using System;
using AbilitySystem;
using EventCallbacks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour {
    
    public float bulletSpeed;
    private Vector3 direction;
    private Rigidbody rigidbody;

    private GameplayAbility ability;
    
    public void Init(GameplayAbility ability) {
        this.ability = ability;
    }
    
    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        direction = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        
        Destroy(gameObject, 3f);
    }

    private void Update() {
        rigidbody.AddForce(direction.normalized * bulletSpeed);
    }
    
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player") == false) 
            return;

        EventSystem<PlayerHitEvent>.FireEvent(new PlayerHitEvent(transform, ability));

    }
}
