using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Filter is a sequence, but also takes a condition. This condition is also a type of BTNode.
 * If the condition fails, no other child of this node will execute.
 * If no condition is given to the filter, the DefaultCondition : BTNode will return SUCCESS
 */
public class Filter : Sequence
{
    //Constructors
    public Filter(List<BTNode> children, BehaviourTree bt) : base(children, bt) { }
    public Filter(List<BTNode> children, BehaviourTree bt, string name, BTNode condition) : base(children, bt, name, condition) { }
    public Filter(List<BTNode> children, BehaviourTree bt, string name) : base(children, bt, name, new DefaultCondition(bt)) { }

    public void AddCondition(BTNode condition) 
    {
        children.Insert(0, condition);
    }
    public void AddAction(BTNode action)
    {
        children.Add(action);
    }

}
