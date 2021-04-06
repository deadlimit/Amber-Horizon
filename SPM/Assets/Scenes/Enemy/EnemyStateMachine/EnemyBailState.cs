using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu(fileName = "Enemy Bail State", menuName = "New Enemy Bail State")]
public class EnemyBailState : State {

    public List<Vector3> fleeLocations;
    
    private Enemy enemy;
    protected override void Initialize() {
        enemy = (Enemy) owner;
    }

    public override void RunUpdate() {
        
        IEnumerator em = MaterialManipulator.Dissolve(enemy.GetComponent<MeshRenderer>().material, .4f, 0.8f);
        
        
    }
    
    
    
}
