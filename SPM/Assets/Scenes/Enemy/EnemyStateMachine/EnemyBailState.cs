using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Bail State", menuName = "New Enemy Bail State")]
public class EnemyBailState : State {

    public List<Vector3> fleeLocations = new List<Vector3>();
    
    private Enemy enemy;
    private Timer dissolveTimer, moveTimer;
    protected override void Initialize() {

        enemy = (Enemy) owner;
        dissolveTimer = new Timer(.3f);
        dissolveTimer.OnTimerReachesZero += TeleportAway;

        moveTimer = new Timer(.8f);
        moveTimer.OnTimerReachesZero += Move;
        enemy.notMoving = false;
    }
    
    public override void RunUpdate() {
        dissolveTimer.Tick(Time.deltaTime);
        moveTimer.Tick(Time.deltaTime);
    }

    private void TeleportAway() => MaterialManipulator.Dissolve(enemy, enemy.GetComponent<MeshRenderer>().material, .4f, 10);

    private void Move() {
        
        Vector3 newLocation = fleeLocations[Random.Range(0, fleeLocations.Count)];
        
        while(Vector3.Distance(enemy.transform.position, newLocation) < 2)
            newLocation = fleeLocations[Random.Range(0, fleeLocations.Count)];
        
        enemy.transform.position = newLocation;
        enemy.notMoving = true;
        enemy.ProximityCast();
        
    }
    
}
