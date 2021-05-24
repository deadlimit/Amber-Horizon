using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInRange : BTNode
{
    private float range;
    //Denna behöver vara en composite sen, för den ska ha barn
    public TargetInRange(BehaviourTree bt) : base(bt) 
    {
        range = bt.GetBlackBoardValue<float>("Range").GetValue();
    }
    public override Status Evaluate()
    {
        float distance = Vector3.Distance(bt.ownerTransform.position, bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue().position);
        
        //Super unsafe get, must only be called on Forager-Type enemies
        if (distance >= range)
            return Status.BH_FAILURE;
        else
        {
            return Status.BH_SUCCESS;
        }

    }
}
