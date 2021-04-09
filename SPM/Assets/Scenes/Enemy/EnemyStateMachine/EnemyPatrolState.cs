using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Patrol State", menuName = "New Enemy Patrol State")]
public class EnemyPatrolState : State {
    
    private Enemy enemy;

    public override void Initialize(StateMachine stateMachine, object owner) {
        enemy = (Enemy) owner;
    }

    public override void Enter() {
        enemy.Invoke(() => enemy.SamplePositionOnNavMesh(enemy.patrolAreaCenter.position, enemy.patrolAreaRadius));
    }

    public override void RunUpdate() {

        if (enemy.ProximityCast(enemy.outerRing)) {
            enemy.NavMeshAgent.ResetPath();
            enemy.stateMachine.ChangeState<EnemyProximityState>();
        }
        
        if (!enemy.NavMeshAgent.hasPath)
            enemy.Invoke(() => enemy.SamplePositionOnNavMesh(enemy.patrolAreaCenter.position, enemy.patrolAreaRadius), Random.Range(2, 5));
        
    }
    
}
