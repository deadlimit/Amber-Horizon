using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Bail State", menuName = "New Enemy Bail State")]
public class EnemyBailState : State {
    
    private Forager forager;

    protected override void Initialize() {
        forager = (Forager) owner;
    }
    
    public override void Enter() {
        forager.Collider.enabled = false;
        forager.Invoke(() => forager.Animator.SetTrigger("Teleport"));
        forager.Invoke(Move, .5f);
    }
    
    private void Move() {
        
        Vector3 random = Random.insideUnitSphere * (forager.outerRing - 1) + forager.transform.position;
        NavMesh.SamplePosition(random, out var hit, 1000, NavMesh.AllAreas);
        
        forager.transform.position = hit.position;
        
        forager.Collider.enabled = true;
        
        if(forager.ProximityCast(forager.innerRing)) 
            forager.stateMachine.ChangeState<EnemyBailState>();
        else if(forager.ProximityCast(forager.outerRing))
            forager.stateMachine.ChangeState<EnemyProximityState>();
        else 
            forager.stateMachine.ChangeState<EnemyPatrolState>();
    }
    
}
