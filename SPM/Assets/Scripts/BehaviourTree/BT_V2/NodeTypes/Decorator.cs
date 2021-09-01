using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : BTNode
{
    protected BTNode m_child;
    public Decorator(BTNode child, BehaviourTree bt) : base(bt)
    {
        m_child = child;
    }

}
