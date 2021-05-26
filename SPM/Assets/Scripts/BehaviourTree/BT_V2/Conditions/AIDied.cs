using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDied : BTNode
{
    public AIDied(BehaviourTree bt) : base(bt) { }
    public override Status Evaluate()
    {
        //skulle istället kunna lyssna efter ett event
        if(!bt.owner.died)
            return Status.BH_FAILURE;

        return Status.BH_SUCCESS;
    }
}
