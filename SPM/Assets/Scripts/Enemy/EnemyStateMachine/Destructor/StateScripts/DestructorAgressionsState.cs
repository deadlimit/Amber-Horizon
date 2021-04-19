using UnityEngine;

[CreateAssetMenu(fileName = "DestructorAgressionsState State", menuName = "New DestructorAgressionsState State")]
public class DestructorAgressionsState : State {

    private Destructor destructor;

    protected override void Initialize() {
        destructor = owner as Destructor;
    }

    public override void Enter() {
        destructor.Pathfinder.agent.isStopped = true;
        destructor.Pathfinder.agent.ResetPath();
        destructor.Animator.SetTrigger("Point");


    }

    public override void RunUpdate() {
        destructor.transform.LookAt(destructor.Target);
    }

}
