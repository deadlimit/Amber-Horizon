using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Destructor Patrol State", menuName = "New Destructor Patrol State")]
public class DestructorPatrolState : State {

    private Destructor destructor;
    
    public float patrolRadius;
    
    protected override void Initialize() {
        destructor = (Destructor) owner;
    }
    public override void Enter() {
        Vector3 newPosition = destructor.Pathfinder.GetSamplePositionOnNavMesh(destructor.transform.position, patrolRadius);
        destructor.Pathfinder.agent.SetDestination(newPosition);
    }

    public override void RunUpdate() {

        if (destructor.ProximityCast(destructor.outerRing)) {
            destructor.Pathfinder.agent.ResetPath();
            destructor.stateMachine.ChangeState<DestructorChargeState>();
        }

        if (destructor.Pathfinder.agent.remainingDistance < .1f) 
            destructor.stateMachine.ChangeState<DestructorIdleState>();
       
    }
    
}
