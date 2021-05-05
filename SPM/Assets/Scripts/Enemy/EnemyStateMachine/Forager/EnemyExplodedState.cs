using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Exploded State", menuName = "EnemyStates/EnemyExplodedState")]
public class EnemyExplodedState : State
{
    Forager forager;
    public override void Initialize(StateMachine stateMachine, object owner)
    {
        forager = (Forager)owner;
        
    }

    public override void Enter()
    {
        Destroy(forager.gameObject, 3f);
    }

}
