using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateLastSeen : BTNode
{
    private Vector3 targetPos; 
    public InvestigateLastSeen(BehaviourTree bt) : base(bt) { }

    public override void OnInitialize()
    {
        Debug.Log("Investigate Last Seen Reached");
        //Os�ker h�mtning och skum kastning, hur st�dar vi detta? 
        BehaviourTree.DataContainer<Vector3> targetContainer = bt.GetBlackBoardValue<Vector3>("LastSeenPosition");
        if (targetContainer != null)
            targetPos = targetContainer.GetValue();

        //Vector3.zero �r v�rt default-value i det h�r fallet, allts� vill vi inte g� dit om det �r detta v�rdet inneh�ller
        //Vill kanske g�ra den h�r kollen utanf�r denna klass, i ett filter
        if (!targetPos.Equals(Vector3.zero))
        {
            bt.owner.Pathfinder.agent.SetDestination(targetPos);
        }
        else
            Debug.Log("Investigate Last Seen Init");
    }

    //
    public override Status Evaluate()
    {
        if (Vector3.Distance(bt.GetBlackBoardValue<Vector3>("LastSeenPosition").GetValue(), Vector3.zero) < 1)
        {
            return Status.BH_FAILURE;
        }
        else if (Vector3.Distance(bt.ownerTransform.position, targetPos) < 1)
        {
            bt.blackboard["LastSeenPosition"] = null;
            return Status.BH_SUCCESS;
        }
        else
            return Status.BH_RUNNING;
    }
}

