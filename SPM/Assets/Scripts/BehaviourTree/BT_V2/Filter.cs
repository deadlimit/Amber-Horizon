using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Filter �r en sequence, med sitt condition som f�rsta barn -> 
 * allts� kommer inga barn k�ras om inte det f�rsta (v�rt condition) returnerar true,
 * och det �r s� filtret fungerar. 
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
