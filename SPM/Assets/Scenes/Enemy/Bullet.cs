using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private PhysicsComponent physics;

    public float BulletSpeed;

    public LayerMask playerLayer;
    
    private Vector3 direction;
    private Timer timeBeforeDissapear;
    
    private void Awake() {
        timeBeforeDissapear = new Timer(1);
        timeBeforeDissapear.OnTimerReachesZero += DestroyThis;
        physics = GetComponent<PhysicsComponent>();
        direction = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
    }

    private void Update() {
        timeBeforeDissapear.Tick(Time.deltaTime);
        physics.AddForce(direction.normalized * BulletSpeed);

        Physics.Raycast(transform.position, transform.forward.normalized, out var hit, .5f,playerLayer);
        
        
        if(hit.collider)
            DestroyThis();
        
    }

    private void DestroyThis() {
        Destroy(gameObject);
    }
    
}
