using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInRange : BTNode
{
    //Denna beh�ver vara en composite sen, f�r den ska ha barn
    public TargetInRange(BehaviourTree bt) : base(bt) { }
    public override Status Evaluate()
    {
        float distance = Vector3.Distance(bt.ownerTransform.position, bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue().position);
        if (distance >= bt.owner.range)
            return Status.BH_FAILURE;
        else
        {
            return Status.BH_SUCCESS;
        }

        //Om vi �r in range, stanna och skjut

    }
}
