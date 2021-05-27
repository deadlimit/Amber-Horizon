using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STest1 : BTNode
{
    public STest1(BehaviourTree bt) : base(bt) { }
    public override Status Evaluate()
    {
        Debug.Log("Test1 Reached");
        return Status.BH_SUCCESS;
    }
}
