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
        //Osäker hämtning och skum kastning, hur städar vi detta? 
        targetPos = (Vector3)bt.blackboard["Target"];
        Debug.Assert(targetPos != Vector3.zero);
        bt.owner.Pathfinder.agent.SetDestination(targetPos);
    }
    //bygger på att target inte är null
    //hårdkodat värde, var vill vi placera det? samma sak i Patrol-scriptet
    public override Status Evaluate()
    {
        if (Vector3.Distance(bt.ownerTransform.position, targetPos) < 1)
        {
            bt.blackboard["Target"] = null;
            return Status.BH_SUCCESS;
        }
        //Tar vi oss hit har det första if-statementet redan körts..? 
        else if(bt.blackboard["Target"] == null)
        {
            return Status.BH_FAILURE;
        }

        else
            return Status.BH_RUNNING;
    }
}
