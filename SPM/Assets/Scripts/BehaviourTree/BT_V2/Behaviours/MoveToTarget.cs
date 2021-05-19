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
    //Tror att det inte �r n�got som avbryter denna nu, s� den ligger kvar h�r och returnerar aldrig n�gon typ av v�rde
    public override Status Evaluate()
    {

        /*if (Vector3.Distance(bt.ownerTransform.position, playerTransform.position) < 1)
        {   bt.owner.Pathfinder.agent.ResetPath();
            return Status.BH_SUCCESS;
        }
        else
            return Status.BH_RUNNING;*/
        //Om den h�r returnerar true skickar den till ShootSequence, 
        //sedan till VisualCheckFilter och sist RootSelector, som d� aldrig utv�rderar n�got annat.

        //Beh�ver vi n�got failure condition? 
        Debug.Log("Move to target returning failure");
        return Status.BH_FAILURE;
    }
}

