using UnityEngine;

[CreateAssetMenu(fileName = "DestructorMeleeState", menuName = "Enemies/Destructor/New DestructorMeleeState State")]
public class DestructorMeleeState : State {

    private Destructor destructor;

    [SerializeField] private float recoveryTime;
    
    protected override void Initialize() {
        destructor = owner as Destructor;
    }

    public override void Enter() {
        destructor.Pathfinder.agent.isStopped = true;
        destructor.Animator.SetTrigger("Melee");
        destructor.Invoke(() => destructor.stateMachine.ChangeState<DestructorChaseState>(), recoveryTime);
    }
    
}
