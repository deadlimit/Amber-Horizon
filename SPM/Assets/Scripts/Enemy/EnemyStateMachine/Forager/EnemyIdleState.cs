using UnityEngine;
[CreateAssetMenu(fileName = "Enemy Idle State", menuName = "New Enemy Idle State")]
public class EnemyIdleState : State {

    private Forager forager;
    
    public override void Initialize(StateMachine stateMachine, object owner) {
        forager = (Forager) owner;
    }

    public override void Enter() {
        Debug.Log("Idle state" + forager.gameObject);
        if(forager.ProximityCast(forager.outerRing))
            forager.stateMachine.ChangeState<EnemyProximityState>();
        
        forager.Pathfinder.agent.ResetPath();
        
        forager.Invoke(() => forager.stateMachine.ChangeState<EnemyPatrolState>(), Random.Range(0, 3));
        
    }
}
