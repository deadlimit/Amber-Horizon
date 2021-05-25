using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Composite
{
    private BTNode condition;
    public Selector(List<BTNode> children, BehaviourTree bt ) : base(children, bt) {}
    public Selector(List<BTNode> children, BehaviourTree bt, string name) : base(children, bt, name)  {}
    public Selector(List<BTNode> children, BehaviourTree bt, string name, BTNode condition) : base(children, bt, name) 
    {
        this.condition = condition;
    }
    public override Status Evaluate()
    {
        if (condition != null)
        {
            if (condition.Tick() == Status.BH_FAILURE)
                return Status.BH_FAILURE;
        }

        foreach (BTNode child in children)
        {
            Status s = child.Tick();
            if (s != Status.BH_FAILURE)
            {
                Debug.Log(name + "recieving: " + s + "child: " + child.name);
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


