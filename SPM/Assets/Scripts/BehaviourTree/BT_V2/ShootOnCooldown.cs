using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootOnCooldown : BTNode
{
    public ShootOnCooldown(BehaviourTree bt) : base(bt) { }
    public override Status Evaluate()
    {
        return Status.BH_FAILURE;
    }
}
