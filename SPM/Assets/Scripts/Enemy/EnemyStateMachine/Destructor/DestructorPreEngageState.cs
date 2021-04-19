using UnityEngine;

[CreateAssetMenu(fileName = "DestructorPreEngageState State", menuName = "New DestructorPreEngageState State")]
public class DestructorPreEngageState : State {

    private Destructor destructor;

    protected override void Initialize() {
        destructor = owner as Destructor;
    }

    public override void Enter() {
        destructor.Animator.SetTrigger("Point");
    }
}
