using System.Security.Cryptography;
using UnityEngine;

[CreateAssetMenu(fileName = "DestructorPatrolState State", menuName = "Enemy States/Destructor/New DestructorPatrolState")]
public class DestructorPatrolState : State {

    private Vector3 originPosition;
    private Destructor destructor;
    
    protected override void Initialize() {
        destructor = owner as Destructor;
        originPosition = destructor.transform.position;
    }

    public override void Enter() {
        destructor.Pathfinder.agent.SetDestination(destructor.Pathfinder.GetSamplePositionOnNavMesh(originPosition, 10, 15));
        Debug.Log(destructor.Pathfinder.agent.destination);
    }

    public override void RunUpdate() {

        if (destructor.ProximityCast(destructor.outerRing)) {
            destructor.stateMachine.ChangeState<DestructorAgressionsState>();
            return;
        }
            
        
        if(destructor.Pathfinder.agent.remainingDistance < .1f)
            destructor.stateMachine.ChangeState<DestructorIdleState>();
        
    }

}
