using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//May seem like a simple duplication of TargetNotNull - and it basically is. Either of these could be inverted and serve both purposes,
//But stuffing another layer of nodes (to fit the inverter) makes the tree itself even more awful to read.
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
