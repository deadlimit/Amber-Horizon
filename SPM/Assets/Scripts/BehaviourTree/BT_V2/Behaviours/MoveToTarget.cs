using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : BTNode
{
    Transform playerTransform;
   public MoveToTarget(BehaviourTree bt) : base(bt)
    {

    }
    public override void OnInitialize()
    {
        Debug.Log("MoveToTarget");
        playerTransform = bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue();

        bt.owner.Pathfinder.agent.SetDestination(playerTransform.position);
    }
    //Evaluate? 
    //Tror att det inte är något som avbryter denna nu, så den ligger kvar här och returnerar aldrig någon typ av värde
    public override Status Evaluate()
    {

        /*if (Vector3.Distance(bt.ownerTransform.position, playerTransform.position) < 1)
        {   bt.owner.Pathfinder.agent.ResetPath();
            return Status.BH_SUCCESS;
        }
        else
            return Status.BH_RUNNING;*/
        //Om den här returnerar true skickar den till ShootSequence, 
        //sedan till VisualCheckFilter och sist RootSelector, som då aldrig utvärderar något annat.

        //Behöver vi något failure condition? 
        Debug.Log("Move to target returning failure");
        return Status.BH_FAILURE;
    }
}

