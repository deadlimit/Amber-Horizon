using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Composite
{
    public Selector(List<BTNode> children, BehaviourTree bt ) : base(children, bt) { }
    public override Status Evaluate()
    {
        foreach (BTNode child in children)
        {
            Status s = child.Tick();
            if (s != Status.BH_FAILURE)
                return s;
            if (children.IndexOf(child) == children.Count)
                return Status.BH_FAILURE;
        }

        return Status.BH_INVALID; //Unexpected loop exit
    }
}


