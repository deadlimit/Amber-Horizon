using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastSeenPosition : BTNode
{
    Vector3 targetPos;
    public LastSeenPosition(BehaviourTree bt) : base(bt) { }

    public override void OnInitialize()
    {
    }
    public override Status Evaluate()
    {
        targetPos = bt.GetBlackBoardValue<Vector3>("LastSeenPosition").GetValue();

        if (targetPos.Equals(Vector3.zero))
        {
            return Status.BH_FAILURE;
        }

        return Status.BH_SUCCESS;
    }
}
