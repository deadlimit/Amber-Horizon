using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    [SerializeField] private State[] states;

    public float outerRing, innerRing;
    
    public LayerMask playerMask;
    public StateMachine stateMachine { get; private set; }

    public Transform target { get; private set; }

    public bool notMoving;
    
    public PhysicsComponent physics { get; private set; }
    public GameObject Bullet;
    
    [HideInInspector] public NavMeshAgent MeshAgent { get; private set; }
    
    private void Awake() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        MeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new StateMachine(this, states);

        notMoving = true;
        physics = GetComponent<PhysicsComponent>();
        
    }
    private void Update() {
        transform.LookAt(target);
        
        if (notMoving)
            ProximityCast();

        stateMachine?.RunUpdate();
    }

    public void ProximityCast() {

        if (Physics.OverlapSphere(transform.position, outerRing, playerMask).Length > 0)
            if (Physics.OverlapSphere(transform.position, innerRing, playerMask).Length > 0 || !physics.isGrounded()) 
                stateMachine.ChangeState<EnemyBailState>();
            else 
                stateMachine.ChangeState<EnemyProximityState>();
        else 
            stateMachine.ChangeState<EnemyDistanceState>();
        
    }

    public void Fire(int shots) {
        for(int i = 0; i < shots; i++)
         Instantiate(Bullet, transform.position + transform.forward, transform.rotation);
    }

}
