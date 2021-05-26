using UnityEngine;

[CreateAssetMenu(fileName = "DestructorChaseState", menuName = "Enemy States/Destructor/New DestructorChaseState")]
public class DestructorChaseState : State {

    private Destructor destructor;

    public float increasedMovementSpeed;
    public float maxDistanceFromOrigin;
    public float minimumDistanceBeforeMelee;
    public float PlayerToFarAway;
    protected override void Initialize() {
        destructor = owner as Destructor;
    }

    public override void Enter() {
        destructor.Pathfinder.agent.speed += increasedMovementSpeed;
        destructor.Pathfinder.agent.isStopped = false;
    }

    public override void RunUpdate() {
        
        if (Vector3.Distance(destructor.originPosition, destructor.transform.position) > maxDistanceFromOrigin) /*||
            Vector3.Distance(destructor.transform.position, destructor.Target.position) > PlayerToFarAway ||
            !destructor.Pathfinder.agent.SetDestination(destructor.Target.position))*/
        {
            destructor.stateMachine.ChangeState<DestructorResetState>();
        }

       /* destructor.Pathfinder.agent.SetDestination(destructor.Target.position);
        destructor.transform.LookAt(destructor.Target);

              
        if(Vector3.Distance(destructor.transform.position, destructor.Target.position) < minimumDistanceBeforeMelee)
            destructor.stateMachine.ChangeState<DestructorMeleeState>();*/
    }

    public override void Exit() {
        destructor.Pathfinder.agent.speed -= increasedMovementSpeed;
    }
}
