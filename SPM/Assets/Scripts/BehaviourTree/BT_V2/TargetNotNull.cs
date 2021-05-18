using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNotNull : BTNode
{
    private Vector3 lastSeen; 
    public TargetNotNull(BehaviourTree bt) : base(bt)  { }
    public override Status Evaluate()
    {
        //Skapar den här kastningen garbage? Det är mycket möjligt
        BehaviourTree.DataContainer target;
        bt.blackboard.TryGetValue("Target", out target);

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
