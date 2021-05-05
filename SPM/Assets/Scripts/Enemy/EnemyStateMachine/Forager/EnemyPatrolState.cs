using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Patrol State", menuName = "New Enemy Patrol State")]
public class EnemyPatrolState : State {
    
    private Forager forager;

    public float patrolRadius;
    private int frame;
    
    public override void Initialize(StateMachine stateMachine, object owner) {
        forager = (Forager) owner;
    }

    public override void Enter() {
        Vector3 newPosition = forager.Pathfinder.GetSamplePositionOnNavMesh(forager.transform.position, patrolRadius, 100);
        forager.Pathfinder.agent.SetDestination(newPosition);
    }

    public override void RunUpdate() {
        frame++;
        if (frame != 10) return;
        frame = 0;

        if (forager.ProximityCast(forager.outerRing) || forager.EnemySeen(forager.outerRing)) 
        {
            forager.Pathfinder.agent.ResetPath();
            forager.stateMachine.ChangeState<EnemyProximityState>();
        }

       if (forager.Pathfinder.agent.remainingDistance < .1f) 
           forager.stateMachine.ChangeState<EnemyIdleState>();
       
    }
    
}
