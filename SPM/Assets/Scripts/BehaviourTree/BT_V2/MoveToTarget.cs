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
    //Tror att det inte �r n�got som avbryter denna nu, s� den ligger kvar h�r och returnerar aldrig n�gon typ av v�rde
    public override Status Evaluate()
    {
        //placeholder
        Debug.Log("Move to target returning failure");
        return Status.BH_FAILURE;
    }
}

