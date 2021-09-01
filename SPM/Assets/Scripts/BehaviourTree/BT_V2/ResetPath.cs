using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPath : BTNode
{
    public ResetPath(BehaviourTree bt) : base(bt) { }
    public override Status Evaluate()
    {
        bt.ownerAgent.ResetPath();
        return Status.BH_SUCCESS;
    }
}
