using UnityEngine;
[CreateAssetMenu(fileName = "Destructor Charge State", menuName = "New Destructor Charge State")]
public class DestructorChargeState : State {

    private Destructor destructor;

    protected override void Initialize() {
        destructor = owner as Destructor;
    }

    public override void Enter() {
        //telegraph genom att slå ihop sina händer
        destructor.Pathfinder.agent.speed += 2;
        destructor.Pathfinder.agent.SetDestination(destructor.Target.position);
    }

    public override void RunUpdate() {
        destructor.transform.LookAt(destructor.Target);
        
        if(Vector3.Distance(destructor.transform.position, destructor.Target.position) < 1f)
            stateMachine.ChangeState<DestructorMeleeState>();

    }
}
