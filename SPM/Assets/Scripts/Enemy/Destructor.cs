using System;
using System.IO;
using UnityEngine;

public class Destructor : Enemy {

    private void Start() {
        stateMachine.ChangeState<DestructorPatrolState>();
    }
    
    private void Update() {
        base.Update();
        
        stateMachine.RunUpdate();
        print(stateMachine.currentState);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        if (!Pathfinder.agent.hasPath) return;
        Gizmos.DrawLine(transform.position, Pathfinder.agent.destination);
        Gizmos.DrawWireSphere(Pathfinder.agent.destination, 1);
        Gizmos.DrawWireSphere(transform.position, outerRing);
    }
}
