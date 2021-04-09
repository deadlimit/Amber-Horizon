using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Bail State", menuName = "New Enemy Bail State")]
public class EnemyBailState : State {
    
    private Enemy enemy;
    private NavMeshAgent navMeshAgent;
    
    protected override void Initialize() {
        enemy = (Enemy) owner;
    }


    public override void Enter() {
        enemy.GetComponent<BoxCollider>().enabled = false;
        enemy.Invoke(() => enemy.Animator.SetTrigger("Teleport"));
        enemy.Invoke(Move, .5f);
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
    }
    
    private void Move() {
        
        Vector3 random = Random.insideUnitSphere * (enemy.outerRing - 1) + enemy.transform.position;
        NavMesh.SamplePosition(random, out var hit, 1000, NavMesh.AllAreas);
        
        enemy.transform.position = hit.position;
        enemy.physics.velocity = Vector3.zero;
        
        enemy.GetComponent<BoxCollider>().enabled = true;
        
        if(enemy.ProximityCast(enemy.innerRing)) 
            enemy.stateMachine.ChangeState<EnemyBailState>();
        else if(enemy.ProximityCast(enemy.outerRing))
            enemy.stateMachine.ChangeState<EnemyProximityState>();
        else 
            enemy.stateMachine.ChangeState<EnemyPatrolState>();
    }
    
}
