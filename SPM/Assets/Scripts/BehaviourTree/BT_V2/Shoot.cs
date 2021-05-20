using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbilitySystem;

public class Shoot : BTNode
{
    public Shoot(BehaviourTree bt) : base(bt) {    }
    public override Status Evaluate()
    {
        if (bt.timerNode.GetFireCooldown() > 0)
        {

            Debug.Log("shoot on cooldown");
            return Status.BH_FAILURE;
        }
        else
        {
            Debug.Log("Shoot!");
            bt.ownerAgent.ResetPath();
            bt.owner.Fire();
            bt.timerNode.SetFireCooldown(2f);
            return Status.BH_SUCCESS;
        }
    }
}
