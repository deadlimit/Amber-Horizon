using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Filter är en sequence, med sitt condition som första barn -> 
 * alltså kommer inga barn köras om inte det första (vårt condition) returnerar true,
 * och det är så filtret fungerar. 
 */
public class Filter : Sequence
{
    public Filter(List<BTNode> children, BehaviourTree bt) : base(children, bt) { }
    public Filter(List<BTNode> children, BehaviourTree bt, string name) : base(children, bt, name) { }
    public void AddCondition(BTNode condition) 
    {
        children.Insert(0, condition);
    }
    public void AddAction(BTNode action)
    {
        children.Add(action);
    }

}
