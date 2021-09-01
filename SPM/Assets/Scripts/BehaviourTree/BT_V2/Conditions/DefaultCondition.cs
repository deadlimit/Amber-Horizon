using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used if no condition is specified for Filter-type nodes
public class DefaultCondition : BTNode
{
    public DefaultCondition(BehaviourTree bt) : base(bt) { }
    public override Status Evaluate()
    {
        return Status.BH_SUCCESS;
    }
}
