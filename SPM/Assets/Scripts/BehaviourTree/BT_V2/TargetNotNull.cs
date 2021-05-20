using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetNotNull : BTNode
{
    private Vector3 lastSeen; 
    public TargetNotNull(BehaviourTree bt) : base(bt)  { }

    public override void OnInitialize()
    {
        Debug.Log("Target not null init");
    }
    public override Status Evaluate()
    {
        //Skapar den här kastningen garbage? Det är mycket möjligt
        bt.blackboard.TryGetValue("Target", out BehaviourTree.DataContainer target);

        if (target == null)
        {
            Debug.Log("Target is null");
            return Status.BH_FAILURE;
        }
        else
        {
            return Status.BH_SUCCESS;
        }
    }
}
