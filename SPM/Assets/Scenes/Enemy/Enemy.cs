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
    
    public PlayerPhysics physics { get; private set; }
    
    [HideInInspector] public NavMeshAgent MeshAgent { get; private set; }
    
    private void Awake() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        MeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new StateMachine(this, states);
        notMoving = true;
        physics = GetComponent<PlayerPhysics>();
    }
    
    private void Update() {

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
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, outerRing);
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, innerRing);
    }
}
