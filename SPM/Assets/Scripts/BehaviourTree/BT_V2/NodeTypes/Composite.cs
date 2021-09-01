using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Composite : BTNode
{
    
    protected List<BTNode> children;
   
    //Constructors
    public Composite(List<BTNode> children, BehaviourTree bt) : base(bt)
    {
        this.children = children;
    }
    public Composite(List<BTNode> children, BehaviourTree bt, string name) : base(bt, name)
    {
        this.children = children;
    }
}
