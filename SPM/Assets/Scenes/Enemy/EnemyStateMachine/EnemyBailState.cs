using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Bail State", menuName = "New Enemy Bail State")]
public class EnemyBailState : State {

    public List<Vector3> fleeLocations = new List<Vector3>();
    
    private Enemy enemy;
    private Timer dissolveTimer, moveTimer;
    protected override void Initialize() {

        enemy = (Enemy) owner;
        dissolveTimer = new Timer(1);
        moveTimer = new Timer(1.1f);
        dissolveTimer.OnTimerReachesZero += TeleportAway;
        moveTimer.OnTimerReachesZero += Move;
    }

    public override void Enter() {
        dissolveTimer.Tick(Time.deltaTime);
    }

    public override void RunUpdate() { }

    private void TeleportAway() {
        MaterialManipulator.Dissolve(enemy, enemy.GetComponent<MeshRenderer>().material, .4f, 20);
    }

    private void Move() {
        int randomLocation = Random.Range(0, fleeLocations.Count);
        enemy.transform.position = fleeLocations[randomLocation];
        enemy.ProximityCast();
    }
    
}
