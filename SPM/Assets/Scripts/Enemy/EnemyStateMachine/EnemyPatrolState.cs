using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Patrol State", menuName = "New Enemy Patrol State")]
public class EnemyPatrolState : State {
    
    private Forager forager;

    public override void Initialize(StateMachine stateMachine, object owner) {
        forager = (Forager) owner;
    }

    public override void Enter() {
        forager.Invoke(() => forager.SamplePositionOnNavMesh(forager.patrolAreaCenter.position, forager.patrolAreaRadius));
    }

    public override void RunUpdate() {

       /* if (forager.ProximityCast(forager.outerRing)) {
            forager.NavMeshAgent.ResetPath();
            forager.stateMachine.ChangeState<EnemyProximityState>();
        }
        
        if (!forager.NavMeshAgent.hasPath)
            forager.Invoke(() => forager.SamplePositionOnNavMesh(forager.patrolAreaCenter.position, forager.patrolAreaRadius), Random.Range(2, 5));
        */
    }
    
}
