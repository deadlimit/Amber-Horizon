using System;
using UnityEngine;

public class Bullet : MonoBehaviour {

    
    public float BulletSpeed;

    public LayerMask playerLayer;
    public float bulletSpeed;
    private Vector3 direction;
    private Rigidbody rigidbody;
    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        direction = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        
        Destroy(gameObject, 3f);
    }

    private void Update() {
        rigidbody.AddForce(direction.normalized * bulletSpeed);
    }
    
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player") == false) return;
        
        print("Hit");
    }
}
