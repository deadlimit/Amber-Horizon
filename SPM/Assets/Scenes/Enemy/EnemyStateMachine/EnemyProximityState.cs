using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Enemy Proximity State", menuName = "New Enemy Proximity State")]
public class EnemyProximityState : State {
    private Enemy enemy;

    private Timer timeBeforeMove;
    
    protected override void Initialize() {
        enemy = (Enemy) owner;
    }
    public override void RunUpdate() {

        if (timeBeforeMove != null)
            timeBeforeMove.Tick(Time.deltaTime);
        else {
            timeBeforeMove = new Timer(1);
            timeBeforeMove.OnTimerReachesZero += Move;
        }
    }

    private void Move() {
        Vector3 directionToPlayer = enemy.transform.position - (enemy.target.position - enemy.transform.position);
        
        enemy.physics.HandleInput(directionToPlayer.normalized * .04f);
    }
}
