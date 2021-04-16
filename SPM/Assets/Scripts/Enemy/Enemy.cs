using System.Collections.Generic;
using UnityEngine;


public abstract class Enemy : MonoBehaviour {

    protected PhysicsComponent physicsComponent;
    protected Animator animator;
    protected AIPathfinder pathfinder;
    
    [SerializeField] protected State[] states;
    
    public StateMachine stateMachine { get; private set; }
    
    private void Awake() {
        stateMachine = new StateMachine(this, states);
        animator = GetComponent<Animator>();
        physicsComponent = GetComponent<PhysicsComponent>();
        pathfinder = GetComponent<AIPathfinder>();
    }

    public abstract void BlackHoleDeath(BlackHole blackHole);
}
