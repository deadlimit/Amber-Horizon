using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDied : BTNode
{
    public AIDied(BehaviourTree bt) : base(bt) { }
    public override Status Evaluate()
    {
        if(!bt.owner.died)
            return Status.BH_FAILURE;

        return Status.BH_SUCCESS;
    }
}
