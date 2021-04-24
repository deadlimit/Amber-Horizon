using UnityEngine;

[CreateAssetMenu(fileName = "DestructorIdleState State", menuName = "Enemy States/Destructor/New DestructorIdleState")]
public class DestructorIdleState : State {

    private Destructor destructor;

    protected override void Initialize() {
        destructor = owner as Destructor;
    }

    public override void Enter() {
        if(destructor.ProximityCast(destructor.outerRing))
            Debug.Log("Pre engage");
        else 
            destructor.Invoke(() => destructor.stateMachine.ChangeState<DestructorPatrolState>(), Random.Range(0, 2));

    }
    
}
