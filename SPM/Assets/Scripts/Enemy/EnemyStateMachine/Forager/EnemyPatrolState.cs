using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Patrol State", menuName = "New Enemy Patrol State")]
public class EnemyPatrolState : State {
    
    private Forager forager;

    public float patrolRadius;
    
    public override void Initialize(StateMachine stateMachine, object owner) {
        forager = (Forager) owner;
    }

    public override void Enter() {
        Vector3 newPosition = forager.Pathfinder.GetSamplePositionOnNavMesh(forager.transform.position, patrolRadius);
        forager.Pathfinder.agent.SetDestination(newPosition);
    }

    public override void RunUpdate() {

       if (forager.ProximityCast(forager.outerRing)) {
            forager.Pathfinder.agent.ResetPath();
            forager.stateMachine.ChangeState<EnemyProximityState>();
       }

       if (forager.Pathfinder.agent.remainingDistance < .1f) 
           forager.stateMachine.ChangeState<EnemyIdleState>();
       
    }
    
}
