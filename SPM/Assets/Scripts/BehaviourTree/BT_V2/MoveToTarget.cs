using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : BTNode
{
   public MoveToTarget(BehaviourTree bt) : base(bt)
    {

    }
    public override void OnInitialize()
    {
        Debug.Log("MoveToTarget");
        Transform playerTransform = bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue();
        bt.owner.Pathfinder.agent.ResetPath();
        bt.owner.Pathfinder.agent.SetDestination(playerTransform.position);
    }
    
}

