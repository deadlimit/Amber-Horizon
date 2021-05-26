using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInRange : BTNode
{
    public TargetInRange(BehaviourTree bt) : base(bt) {}
    public override void OnInitialize()
    {
//        bt.owner.Animator.SetBool("PlayerInRange", false);
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
            //Destructor PlayerInRange animator parameter i set in DestructorAttack node instead of here
            return Status.BH_SUCCESS;
        }

    }
}
