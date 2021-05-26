using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastSeenPosition : BTNode
{
    Vector3 targetPos;
    public LastSeenPosition(BehaviourTree bt) : base(bt) { }

    public override void OnInitialize()
    {
        targetPos = bt.GetBlackBoardValue<Vector3>("LastSeenPosition").GetValue();
        Debug.Log("LastSeenPosition.cs targetPos: " + targetPos);
    }
    public override Status Evaluate()
    {
        if (targetPos.Equals(Vector3.zero))
        {
            return Status.BH_FAILURE;
        }

        return Status.BH_SUCCESS;
    }
}
