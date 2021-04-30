using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Composite : BTNode
{
    /* TODO;
     void AddChild(Behaviour child);
     void RemoveChild(Behaviour child);
     void clearChildren() 
     */
    protected List<BTNode> children;
    public Composite(List<BTNode> children, BehaviourTree bt) : base(bt)
    {
        this.children = children;
    }
}
