using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInRange : BTNode
{
    //Denna behöver vara en composite sen, för den ska ha barn
    public TargetInRange(BehaviourTree bt) : base(bt) {}
    public override void OnInitialize()
    {
        Debug.Log("TargetInRange init");
    }
    public override Status Evaluate()
    {
        float distance = Vector3.Distance(bt.ownerTransform.position, bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue().position);
        bt.owner.Animator.SetFloat("DistanceToTarget", distance);

        if (distance >= bt.owner.AttackRange)
        {
            Debug.Log("Out of range");
            return Status.BH_FAILURE;
        }
        else
        {
            Debug.Log("In range");
            return Status.BH_SUCCESS;
        }

    }
}
