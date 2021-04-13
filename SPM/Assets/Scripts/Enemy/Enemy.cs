using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : MonoBehaviour, IBlackHoleBehaviour {

    public PhysicsComponent physicsComponent { get; set; }
    public Animator animator { get; set; }
    public AIPathfinder pathfinder { get; set; }
    public Transform target { get; set; }
    public CapsuleCollider collider { get; set; }
    
    [SerializeField] private State[] states;
    
    public StateMachine stateMachine { get; private set; }
    
    public void Awake() {
        stateMachine = new StateMachine(this, states);
        animator = GetComponent<Animator>();
        physicsComponent = GetComponent<PhysicsComponent>();
        collider = GetComponent<CapsuleCollider>();
        pathfinder = GetComponent<AIPathfinder>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update() {
        animator.SetFloat("Velocity", Vector3.Distance(transform.position, pathfinder.agent.destination));
    }
    
    public virtual void BlackHoleBehaviour(BlackHole blackHole) { Debug.Log("hello");}
}
