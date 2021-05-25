using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbilitySystem;

public class Shoot : BTNode
{
    private float shootCD;
    
    public Shoot(BehaviourTree bt) : base(bt) 
    {
        shootCD = bt.GetBlackBoardValue<float>("FireCooldown").GetValue();
    }
    public override Status Evaluate()
    {
        if (bt.timerNode.GetAttackCooldown() > 0)
        {
            return Status.BH_FAILURE;
        }
        else
        {
            bt.ownerAgent.ResetPath();
            bt.owner.Animator.SetTrigger("Shoot");
            bt.timerNode.SetAttackCooldown(shootCD);
            return Status.BH_SUCCESS;
        }
    }
}
