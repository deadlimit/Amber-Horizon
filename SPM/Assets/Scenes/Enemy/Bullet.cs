using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private PhysicsComponent physics;

    public float BulletSpeed;

    public LayerMask playerLayer;
    
    private Vector3 direction;
    
    private void Awake() {
        physics = GetComponent<PhysicsComponent>();
        direction = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        Destroy(gameObject, 3f);
    }

    private void Update() {
        physics.AddForce(direction.normalized * BulletSpeed);

        Physics.Raycast(transform.position, transform.forward.normalized, out var hit, 1,playerLayer);
        
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<Health>().TakeDamage();

        Destroy(gameObject);

    }
}
