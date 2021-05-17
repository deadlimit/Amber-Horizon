using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNotNull : BTNode
{
    public TargetNotNull(BehaviourTree bt) : base(bt)  { }
    public override Status Evaluate()
    {
        object target;
        bt.blackboard.TryGetValue("Target", out target);
        if (target == null)
        {
            //Debug.Log("returning FAILURE");
            return Status.BH_FAILURE;
        }
        else
        {
            //Debug.Log("returning SUCCESS");
            return Status.BH_SUCCESS;
        }
    }
}
