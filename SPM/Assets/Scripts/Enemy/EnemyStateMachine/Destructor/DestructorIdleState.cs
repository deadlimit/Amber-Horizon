using UnityEngine;
[CreateAssetMenu(fileName = "Destructor Idle State", menuName = "New Destructor Idle State")]

//TODO Måste hitta ett sätt att slippa hålla privat referens till typen, kan inte återanvända kod från andra states annars

public class DestructorIdleState : State {

    private Destructor destructor;

    protected override void Initialize() {
        destructor = (Destructor) owner;
    }

    public override void Enter() {
        if(destructor.ProximityCast(destructor.outerRing))
            destructor.stateMachine.ChangeState<DestructorChargeState>();
        
        destructor.Pathfinder.agent.ResetPath();
        
        destructor.Invoke(() => destructor.stateMachine.ChangeState<DestructorPatrolState>());
        
    }
}
