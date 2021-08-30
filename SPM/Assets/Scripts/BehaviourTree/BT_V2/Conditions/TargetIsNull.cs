using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIsNull : BTNode
{
    public TargetIsNull(BehaviourTree bt) : base(bt) { }

    public override Status Evaluate()
    {
        bt.blackboard.TryGetValue("Target", out BehaviourTree.DataContainer target);
        if (target == null)
        {
            return Status.BH_SUCCESS;
        }
        else
        {
            return Status.BH_FAILURE;
        }
    }

}
