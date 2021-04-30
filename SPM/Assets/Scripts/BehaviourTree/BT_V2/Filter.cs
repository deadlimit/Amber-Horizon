using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filter : Sequence
{
    //detta �r lagg, tar emot och skickar vidare lista f�r att kunna kompilera atm
    public Filter(List<BTNode> children, BehaviourTree bt) : base(children, bt) { }
    public void AddCondition(BTNode condition) 
    {
        children.Insert(0, condition);
    }
    public void AddAction(BTNode action)
    {
        children.Add(action);
    }

}
