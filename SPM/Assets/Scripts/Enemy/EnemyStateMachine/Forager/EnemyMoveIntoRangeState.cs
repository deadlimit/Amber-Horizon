using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy States", menuName = "MoveIntoRange State")]
public class EnemyMoveIntoRangeState : State
{ 
    private Forager forager; 
    protected override void Initialize()
    {
        forager = (Forager)owner; 
    }
    public override void Enter()
    {
        forager.Pathfinder.agent.ResetPath();
        Debug.Log(forager.gameObject + "moving towards target");
        forager.Pathfinder.agent.SetDestination(forager.Target.transform.position);
        forager.Pathfinder.agent.speed *= 2;
    }
    public override void RunUpdate()
    {
        if (Vector3.Distance(forager.Target.transform.position, forager.transform.position) < forager.range)
            stateMachine.ChangeState<EnemyProximityState>();

        forager.transform.LookAt(forager.Target);
    }
    public override void Exit()
    {
        forager.Pathfinder.agent.speed /= 2;
        Debug.Log("leaving move into range state");
    }
}
