using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : BTNode
{
    Transform playerTransform;
    int frameCounter = 0;
   public MoveToTarget(BehaviourTree bt) : base(bt)
    {

    }
    public override void OnInitialize()
    {
        playerTransform = bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue();
    }
    

    public override Status Evaluate()
    {
        UpdateTargetPosition();

        if (ReachedTarget())
        {
            bt.owner.Pathfinder.agent.ResetPath();
            return Status.BH_SUCCESS;
        }
        else if (Vector3.Distance(bt.ownerTransform.position, playerTransform.position) >= bt.owner.VisualRange)
            return Status.BH_FAILURE;
        else
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

    //S�tt destination var tionde frame
    private void UpdateTargetPosition()
    {
        if (frameCounter % 20 == 0)
        {
            bt.owner.Pathfinder.agent.SetDestination(playerTransform.position);
            frameCounter = 0;
        }
        else frameCounter++;
    }
}

