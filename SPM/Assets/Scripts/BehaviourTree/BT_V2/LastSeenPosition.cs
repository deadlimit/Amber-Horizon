using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastSeenPosition : BTNode
{
    BehaviourTree.DataContainer<Vector3> targetContainer;
    public LastSeenPosition(BehaviourTree bt) : base(bt) { }

    public override void OnInitialize()
    {
        targetContainer = bt.GetBlackBoardValue<Vector3>("LastSeenPosition");
    }
    public override Status Evaluate()
    {
        if (targetContainer.GetValue().Equals(Vector3.zero))
            return Status.BH_FAILURE;
        return Status.BH_SUCCESS;
    }
}
