using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeater : Decorator
{
    //First node in the BT to keep things running
    public Repeater(BTNode child, BehaviourTree bt) : base(child, bt) { }
    public override Status Evaluate()
    {
        m_child.Evaluate();
        return Status.BH_RUNNING;
    }
}

