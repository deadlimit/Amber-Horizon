using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateLastSeen : BTNode
{
    private Vector3 targetPos; 
    public InvestigateLastSeen(BehaviourTree bt) : base(bt) { }

    public override void OnInitialize()
    {
        //Condition har redan tittat att det inte är null/Vector3.zero
      targetPos = bt.GetBlackBoardValue<Vector3>("LastSeenPosition").GetValue();
      bt.owner.Pathfinder.agent.SetDestination(targetPos);
        Debug.Log("moving to LSP: " +targetPos);
    }

    //
    public override Status Evaluate()
    {
        if (targetPos.Equals(Vector3.zero))
        {
            Debug.Log("LastSeen == Vector3.zero, returning failure..");
            return Status.BH_FAILURE;
        }
        if (ReachedTarget())
        {
            Debug.Log("Distance: " + Vector3.Distance(bt.GetBlackBoardValue<Vector3>("LastSeenPosition").GetValue(), Vector3.zero));
            Debug.Log("Investigate Last Seen returning SUCCESS");
            bt.GetBlackBoardValue<Vector3>("LastSeenPosition").SetValue(Vector3.zero);
        }

        else
            Debug.Log("InvestigateLastSeen Running");
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

