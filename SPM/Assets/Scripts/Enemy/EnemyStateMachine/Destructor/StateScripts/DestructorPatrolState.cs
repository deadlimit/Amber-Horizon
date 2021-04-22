using System.Security.Cryptography;
using UnityEngine;

[CreateAssetMenu(fileName = "DestructorPatrolState State", menuName = "Enemy States/Destructor/New DestructorPatrolState")]
public class DestructorPatrolState : State {
    
    private Destructor destructor;
    
    protected override void Initialize() {
        destructor = owner as Destructor;
    }

    public override void Enter() {
        destructor.Animator.SetBool("PlayerInRange", false);
        destructor.Pathfinder.agent.SetDestination(destructor.Pathfinder.GetSamplePositionOnNavMesh(destructor.originPosition, 10, 15));
        //Debug.Log(destructor.Pathfinder.agent.destination);
    }

    public override void RunUpdate() {

        if (destructor.ProximityCast(destructor.outerRing)) {
            destructor.Animator.SetBool("PlayerInRange", true);
            destructor.stateMachine.ChangeState<DestructorAgressionsState>();
            return;
        }
        
        if(destructor.Pathfinder.agent.remainingDistance < .1f)
            destructor.stateMachine.ChangeState<DestructorIdleState>();
        
    }

}
