using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dest_InvestigateLastSeen : BTNode
{
    //Script is identical to InvestigateLastSeen but for the animator parameter that is set when succeeding
    //Not quite sure this is the way i want to do things, but im experimenting
    private Vector3 targetPos; 
    public Dest_InvestigateLastSeen(BehaviourTree bt) : base(bt) { }

    public override void OnInitialize()
    {
      //Preceding condition makes sure that this value is not null
      targetPos = bt.GetBlackBoardValue<Vector3>("LastSeenPosition").GetValue();
      bt.owner.Pathfinder.agent.SetDestination(targetPos);
    }

    //
    public override Status Evaluate()
    {
        if (targetPos.Equals(Vector3.zero))
        {
            return Status.BH_FAILURE;
        }
        if (ReachedTarget())
        {
            bt.owner.Animator.SetBool("HasPositionToInvestigate", false);
            bt.GetBlackBoardValue<Vector3>("LastSeenPosition").SetValue(Vector3.zero);
        }
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

