using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Patrol State", menuName = "New Enemy Patrol State")]
public class EnemyPatrolState : State {

    private NavMeshAgent navMeshAgent;
    private Enemy enemy;

    
    
    public override void Initialize(StateMachine stateMachine, object owner) {
        enemy = (Enemy) owner;
    }

    public override void Enter() {
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
    }


    public override void RunUpdate() {
        
    }
}
