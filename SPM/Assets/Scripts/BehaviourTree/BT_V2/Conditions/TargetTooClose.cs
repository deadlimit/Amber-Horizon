using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//behaviour to determine when ranged enemies should attempt to flee (in this particular case that means teleport a small distance away) 
public class TargetTooClose : BTNode
{
    public TargetTooClose(BehaviourTree bt) : base(bt) { }

    public override Status Evaluate()
    {
        if (DistanceToPlayer() < bt.GetBlackBoardValue<float>("FleeDistance").GetValue())
            return Status.BH_SUCCESS;
        else
            return Status.BH_FAILURE;
    }

    private float DistanceToPlayer()
    {
       return  Vector3.Distance(bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue().position, bt.ownerTransform.position);
    }
}
