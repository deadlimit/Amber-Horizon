using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTargetNull : BTNode
{
    //OBS!
    //den här behövs inte alls användas på lägre prio, om vi går dit har vi ju redan tittat på target!!
    //
    public IsTargetNull(BehaviourTree bt) : base(bt) { }

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
