using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Patrol State", menuName = "New Enemy Patrol State")]
public class EnemyPatrolState : State {
    
    private Forager forager;

    public override void Initialize(StateMachine stateMachine, object owner) {
        forager = (Forager) owner;
    }

    public override void Enter() {
        Vector3 newPosition = forager.pathfinder.GetSamplePositionOnNavMesh(forager.transform.position, forager.patrolAreaRadius);
        forager.pathfinder.agent.SetDestination(newPosition);
    }

    public override void RunUpdate() {

       if (forager.ProximityCast(forager.outerRing)) {
            forager.pathfinder.agent.ResetPath();
            forager.stateMachine.ChangeState<EnemyProximityState>();
       }

       if (forager.pathfinder.agent.remainingDistance < .1f) 
           forager.stateMachine.ChangeState<EnemyIdleState>();
       
    }
    
}
