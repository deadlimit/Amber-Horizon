using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTooClose : BTNode
{
    private Transform playerTransform;
    public TargetTooClose(BehaviourTree bt) : base(bt) { }

    public override Status Evaluate()
    {
        if (DistanceToPlayer() < bt.owner.FleeDistance)
            return Status.BH_SUCCESS;
        else
            return Status.BH_FAILURE;
    }

    private float DistanceToPlayer()
    {
       return  Vector3.Distance(bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue().position, bt.ownerTransform.position);
    }
}
