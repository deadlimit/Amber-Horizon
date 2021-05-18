using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investigate : BTNode
{
    private Vector3 targetPos;
    public Investigate(BehaviourTree bt) : base(bt)
    {
    }

    public override void OnInitialize()
    {
        Debug.Log("Investigate Init");
        //Os�ker h�mtning och skum kastning, hur st�dar vi detta? 
        BehaviourTree.DataContainer<Vector3> targetContainer = bt.GetBlackBoardValue<Vector3>("Target");
        if(targetContainer != null)
            targetPos = targetContainer.GetValue();
        bt.owner.Pathfinder.agent.SetDestination(targetPos);
    }
    //bygger p� att target inte �r null
    //h�rdkodat v�rde, var vill vi placera det? samma sak i Patrol-scriptet
    //h�r nullar vi DataContainer
    public override Status Evaluate()
    {
        if (Vector3.Distance(bt.ownerTransform.position, targetPos) < 1)
        {
            bt.blackboard["Target"] = null;
            return Status.BH_SUCCESS;
        }
        //Tar vi oss hit har det f�rsta if-statementet redan k�rts
        else if(bt.blackboard["Target"] == null)
        {
            return Status.BH_FAILURE;
        }

        else
            return Status.BH_RUNNING;
    }
}
