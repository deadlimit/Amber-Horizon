using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbilitySystem;

public class Shoot : BTNode
{
    private float shootCD;
    
    public Shoot(BehaviourTree bt) : base(bt) 
    {
        shootCD = bt.owner.GetFireCooldown();
    }
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
            bt.owner.Fire(bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue());
            bt.timerNode.SetFireCooldown(shootCD);
            return Status.BH_SUCCESS;
        }
    }
}
