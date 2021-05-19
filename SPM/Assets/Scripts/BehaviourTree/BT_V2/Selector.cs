using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Composite
{
    public Selector(List<BTNode> children, BehaviourTree bt ) : base(children, bt) { }
    public Selector(List<BTNode> children, BehaviourTree bt, string name) : base(children, bt, name) { }
    public override Status Evaluate()
    {
        foreach (BTNode child in children)
        {
            Status s = child.Tick();
            if (s != Status.BH_FAILURE)
            {
                Debug.Log(name + "recieving: " +s + "child: " + child.name);
                return s;
            }
            if (children.IndexOf(child) == children.Count -1)
            {
                return Status.BH_FAILURE;
            }
        }

        return Status.BH_INVALID; //Unexpected loop exit
    }
}


