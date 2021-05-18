using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTargetNull : BTNode
{
    //OBS!
    //den h�r beh�vs inte alls anv�ndas p� l�gre prio, om vi g�r dit har vi ju redan tittat p� target!!
    //
    public IsTargetNull(BehaviourTree bt) : base(bt)
    {

    }

    public override Status Evaluate()
    {
        BehaviourTree.DataContainer target;
        bt.blackboard.TryGetValue("Target", out target);
        if (target == null)
        {
            Debug.Log("returning success");
            return Status.BH_SUCCESS;
        }
        else
        {
            Debug.Log("returning failure" );
            return Status.BH_FAILURE;
        }
    }

}
