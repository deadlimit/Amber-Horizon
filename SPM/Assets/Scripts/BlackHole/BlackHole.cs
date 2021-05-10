using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public LayerMask physicsLayerMask;
    public LayerMask collisionMask;
    public float GravitationalPull;
    public float gravity = 10f;
    public Vector3 velocity;
    public float airResistance = 0f;
    
    public GameObject sphereEffect;
    public float maxRadius;

    public Transform center;
    
    SphereCollider coll;
    public BoxCollider centerColl;

    private bool useGravity = true;
    private float terminalDistance = 0.5f;
    
    private float gravitationalPull;

    public float Lifetime;
    
    private Animator animator;
    
    private void Awake() {
        gravitationalPull = GravitationalPull;
        GravitationalPull = 0;
        
        coll = GetComponent<SphereCollider>();
        centerColl = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();

        animator.SetTrigger("Spawn");
        
        this.Invoke(() => {
            animator.SetTrigger("Despawn");
        }, Lifetime);
    }
    // Update is called once per frame
    void Update() {
        
        GravitationDrag();
        //CheckCenterCollision();
        
        velocity *= Mathf.Pow(airResistance, Time.deltaTime);
        transform.Translate(velocity * Time.deltaTime);
    }
    private void GravitationDrag() {
        
        Collider[] hitcoll = Physics.OverlapSphere(transform.position, coll.radius, physicsLayerMask);
        
        foreach (Collider collider in hitcoll) {
            
            PhysicsComponent physicsComponent = collider.GetComponent<PhysicsComponent>();   

            if (physicsComponent) {
                
                if (Vector3.Distance(transform.position, collider.transform.position) < terminalDistance) {
                    physicsComponent.StopVelocity();
                }
                else
                    physicsComponent.BlackHoleGravity(this);
                
            }
            
            IBlackHoleBehaviour blackHoleBehaviour = collider.GetComponent<IBlackHoleBehaviour>();
            if (blackHoleBehaviour != null) 
            {
                blackHoleBehaviour?.BlackHoleBehaviour(this);
            }
            
        }
        
        if (useGravity)
        {
            Collider[] boxHitColl =
            Physics.OverlapBox(transform.position, centerColl.size / 2, Quaternion.identity, collisionMask);
            if (boxHitColl.Length > 0)
            {
                velocity = Vector3.zero;
                useGravity = false;
            }
            
            velocity += Vector3.down * Time.deltaTime * gravity;
        }
        
        velocity *= Mathf.Pow(airResistance, Time.deltaTime);
        transform.Translate(velocity * Time.deltaTime);
    }

    private void Die() => Destroy(gameObject);
    private void TurnOnGravitationPull() => GravitationalPull = gravitationalPull;

    private void StartParticleEffect() {
        GetComponentInChildren<ParticleSystem>().Play();
    }
    
}
