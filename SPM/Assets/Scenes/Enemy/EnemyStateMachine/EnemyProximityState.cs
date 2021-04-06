using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Enemy Proximity State", menuName = "New Enemy Proximity State")]
public class EnemyProximityState : State {
    public float radius;
    private Enemy enemy;

    protected override void Initialize() {
        enemy = (Enemy) owner;
    }

    public override void RunUpdate() {
        Debug.Log("proximity state");

    }
}
