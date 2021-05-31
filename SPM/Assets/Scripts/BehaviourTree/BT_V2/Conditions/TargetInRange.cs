using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInRange : BTNode
{
    public TargetInRange(BehaviourTree bt) : base(bt) {}

    public override Status Evaluate()
    {
        float distance = Vector3.Distance(bt.ownerTransform.position, bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue().position);
        bt.ownerAgent.enabled = true;
        if (distance >= bt.owner.AttackRange)
        {
            bt.ownerAgent.ResetPath();
            Debug.Log("Target in Rnage FAILURE");
            Debug.Log("Distance to player: " + distance);          
            return Status.BH_FAILURE;
        }
        else
        {
            //Destructor PlayerInRange animator parameter i set in DestructorAttack node instead of here
            return Status.BH_SUCCESS;
        }

    }
}
