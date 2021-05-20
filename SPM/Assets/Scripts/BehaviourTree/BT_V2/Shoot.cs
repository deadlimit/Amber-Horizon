using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbilitySystem;

public class Shoot : BTNode
{
    public Shoot(BehaviourTree bt) : base(bt) {    }
    public override Status Evaluate()
    {

        bool b = bt.ownerGAS.TryActivateAbilityByTag(GameplayTags.AttackTag);
        Debug.Log("TryActivate : " + b);
        if (b)
        {
            bt.owner.Pathfinder.agent.ResetPath();
            return Status.BH_SUCCESS;
        }
        return Status.BH_FAILURE;
    }
}
