using UnityEngine;

[CreateAssetMenu(fileName = "DestructorMeleeState State", menuName = "New DestructorMeleeState State")]
public class DestructorMeleeState : State {

    private Destructor destructor;

    protected override void Initialize() {
        destructor = owner as Destructor;
    }

    public override void Enter() {
        destructor.Animator.SetTrigger("Melee");
    }

    public override void RunUpdate() {
        destructor.transform.LookAt(destructor.Target);
    }

}
