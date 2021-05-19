using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STest3 : BTNode
{
    public STest3(BehaviourTree bt) : base(bt) { }
    public override Status Evaluate()
    {
        Debug.Log("Test3 Reached");
        return Status.BH_FAILURE;
    }
}

