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
        
        if (bt.GetBlackBoardValue<Vector3>("Target") != null)
            bt.owner.Pathfinder.agent.SetDestination(targetPos);
    }

    public override Status Evaluate()
    {

        if (ReachedTarget())
        {
            Debug.Log("Blackboard 'Target' set to null ");
            bt.blackboard["Target"] = null;
            return Status.BH_SUCCESS;
        }

        else if (bt.blackboard["Target"] == null)
        {
            return Status.BH_FAILURE;
        }

        else
            Debug.Log("Investigate running");
            return Status.BH_RUNNING;
    }
    private bool ReachedTarget()
    {
        if (!bt.owner.Pathfinder.agent.pathPending)
        {
            if (bt.owner.Pathfinder.agent.remainingDistance <= bt.owner.Pathfinder.agent.stoppingDistance)
            {
                if (!bt.owner.Pathfinder.agent.hasPath || bt.owner.Pathfinder.agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }
 
}
