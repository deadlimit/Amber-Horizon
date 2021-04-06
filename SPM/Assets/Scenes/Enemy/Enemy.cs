using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    [SerializeField] private State[] states;

    private LayerMask playerMask;
    private StateMachine stateMachine;
    private bool hasPath;
    private Transform target;
    
    
    [HideInInspector] public NavMeshAgent MeshAgent { get; private set; }
    
    private void Awake() {
        MeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new StateMachine(this, states);
    }

    
    

    private void Update() {

        Physics.SphereCast(transform.position, 10, transform.forward.normalized, out var hit, 10, playerMask);
        
        if(hit.collider)
            Debug.Log("Player found");

        stateMachine?.RunUpdate();
    }

    private void OnDrawGizmos() {
        Physics.SphereCast(transform.position, 20, transform.forward.normalized, out var hit, 10, playerMask);
        Gizmos.DrawSphere(hit.point, 20);
    }
}
