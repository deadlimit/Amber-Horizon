using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : BTNode
{
    private Transform playerTransform;
    private int frameCounter = 0;
    private int framesPerDestinationUpdate = 20;
    public MoveToTarget(BehaviourTree bt) : base(bt){}
    public override void OnInitialize()
    {
        bt.ownerAgent.enabled = true;
        playerTransform = bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue();
    }
    
    public override Status Evaluate()
    {
        UpdateTargetPosition();

        if (Vector3.Distance(bt.ownerTransform.position, playerTransform.position)< bt.owner.AttackRange)
        {
            bt.owner.Pathfinder.agent.ResetPath();
            return Status.BH_SUCCESS;
        }

        else if (Vector3.Distance(bt.ownerTransform.position, playerTransform.position) >= bt.owner.VisualRange)
            return Status.BH_FAILURE;

        else
            return Status.BH_RUNNING;

    }

    private void UpdateTargetPosition()
    {
        if (frameCounter % framesPerDestinationUpdate == 0)
        {
            bt.owner.Pathfinder.agent.SetDestination(playerTransform.position);
            frameCounter = 0;
        }
        else frameCounter++;
    }
}

