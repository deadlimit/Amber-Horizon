using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STest2 : BTNode
{
    public STest2(BehaviourTree bt) : base(bt) { }
    public override Status Evaluate()
    {
        Debug.Log("Test2 Reached");
        return Status.BH_SUCCESS;
    }
}

