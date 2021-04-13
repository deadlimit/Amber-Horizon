using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Forager : Enemy {
    
    public float patrolAreaRadius;
    public float outerRing, innerRing;
    public LayerMask playerMask;

    
    [HideInInspector] public BlackHole activeBlackHole;

    private void Awake() {
        base.Awake();
    }
    
    private void Update() {
        base.Update();
        if(ProximityCast(outerRing))
            stateMachine.ChangeState<EnemyProximityState>();
        
        stateMachine?.RunUpdate();
    }

    public bool ProximityCast(float radius) {
        return Physics.OverlapSphere(transform.position, radius, playerMask).Length > 0;
    }
    
    public override void BlackHoleDeath(BlackHole blackHole) {
        if (activeBlackHole) return;
        activeBlackHole = blackHole;
        stateMachine.ChangeState<EnemyDeathState>();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        if (pathfinder != null && pathfinder.agent.hasPath) {
            Gizmos.DrawLine(transform.position, pathfinder.agent.destination);
            Gizmos.DrawWireSphere(pathfinder.agent.destination, .5f);
        }
        
    }
}
