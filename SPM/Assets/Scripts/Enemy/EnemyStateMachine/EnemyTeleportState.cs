using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Enemy Teleport State", menuName = "New Enemy Teleport State")]
public class EnemyTeleportState : State {

    private Forager forager;

    public float distanceFromPlayer;
    
    protected override void Initialize() {
        forager = (Forager) owner;
    }

    public override void Enter() {
        forager.animator.SetTrigger("Teleport");
        Vector3 randomPosition = forager.transform.position + (Vector3)Random.insideUnitCircle * distanceFromPlayer;
        forager.transform.position = randomPosition;
        forager.stateMachine.ChangeState<EnemyProximityState>();
    }
}
