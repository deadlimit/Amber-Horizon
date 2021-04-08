using System;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Proximity State", menuName = "New Enemy Proximity State")]
public class EnemyProximityState : State {
    private Enemy enemy;

    private Timer timeBeforeMove;

    public float FireCooldown;
    private float nextFire;
    
    protected override void Initialize() {
        enemy = (Enemy) owner;
    }
    public override void RunUpdate() {
        enemy.transform.LookAt(enemy.target);
        if (timeBeforeMove != null)
            timeBeforeMove.Tick(Time.deltaTime);
        else {
            timeBeforeMove = new Timer(1);
            timeBeforeMove.OnTimerReachesZero += Move;
        }

        if (nextFire < Time.time && CanSeePlayer()) {
            enemy.Fire(1);
            nextFire = FireCooldown + Time.time;
        }
            
    }

    private void Move() {
        enemy.physics.AddForce(-enemy.transform.forward * .02f);
    }

    private bool CanSeePlayer() {
        return Physics.Raycast(enemy.transform.position, enemy.target.position - enemy.transform.position, out var hit, Mathf.Infinity, LayerMask.GetMask("Ground"));
    }
    
}
