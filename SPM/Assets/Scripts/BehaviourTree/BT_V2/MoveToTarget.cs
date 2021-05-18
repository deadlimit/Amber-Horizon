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
    //Evaluate? 
    //Tror att det inte är något som avbryter denna nu, så den ligger kvar här och returnerar aldrig någon typ av värde
    public override Status Evaluate()
    {
        //placeholder
        Debug.Log("Move to target returning failure");
        return Status.BH_FAILURE;
    }
}

