using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNotNull : BTNode
{
    public TargetNotNull(BehaviourTree bt) : base(bt)  { }

    public override Status Evaluate()
    {
        bt.blackboard.TryGetValue("Target", out BehaviourTree.DataContainer target);

        if (target == null)
        {
            return Status.BH_FAILURE;
        }
        else
        {
            return Status.BH_SUCCESS;
        }
    }
}
