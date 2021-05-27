using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOwner : BTNode
{
    public DestroyOwner(BehaviourTree bt) : base(bt) { }
    public override Status Evaluate()
    {
        bt.enabled = false;
        Object.Destroy(bt.owner.gameObject);
        return Status.BH_SUCCESS;
    }
}
