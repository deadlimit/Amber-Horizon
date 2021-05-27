using UnityEngine;

[CreateAssetMenu(fileName = "DestructorAgressionsState", menuName = "Enemies/Destructor/New DestructorAgressionsState")]
public class DestructorAgressionsState : State {

    private Destructor destructor;

    protected override void Initialize() {
        destructor = owner as Destructor;
    }

    public override void Enter() {
        destructor.Pathfinder.agent.isStopped = true;
        destructor.Pathfinder.agent.ResetPath();
        destructor.Animator.SetTrigger("Point");
        destructor.Animator.SetBool("PlayerInRange", true);
        
        destructor.Invoke(() => destructor.stateMachine.ChangeState<DestructorChaseState>(), 2);
    }

    public override void RunUpdate() {
        destructor.transform.LookAt(destructor.Target);
    }

    public override void Exit() {
        destructor.Pathfinder.agent.isStopped = false;
    }
}
