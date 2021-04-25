using UnityEngine;

[CreateAssetMenu(fileName = "DestructorResetState", menuName = "New DestructorResetState")]
public class DestructorResetState : State {

    private Destructor destructor;
    
    protected override void Initialize() {
        destructor = owner as Destructor;
    }

    public override void Enter() {
        destructor.Pathfinder.agent.SetDestination(destructor.originPosition);
        destructor.Animator.SetBool("PlayerInRange", false);
    }

    public override void RunUpdate() {
        if(destructor.Pathfinder.agent.remainingDistance < 1)
            destructor.stateMachine.ChangeState<DestructorIdleState>();
        
        if(destructor.ProximityCast(destructor.outerRing))
            destructor.stateMachine.ChangeState<DestructorAgressionsState>();
    }

}
