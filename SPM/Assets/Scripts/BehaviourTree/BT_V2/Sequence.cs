using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Composite
{ 
    private int currentNode = 0;
    private BTNode condition;

    //Constructors
    public Sequence(List<BTNode> children, BehaviourTree bt) : base(children, bt) { condition = new DefaultCondition(bt); }
    public Sequence(List<BTNode> children, BehaviourTree bt, string name, BTNode condition) : base(children, bt, name) 
    { 
        this.condition = condition;
    }

    public override Status Evaluate()
    {
        if (condition.Tick() == Status.BH_FAILURE)
            return Status.BH_FAILURE;

        //Return only when a child returns RUNNING or FAILURE
        if (currentNode < children.Count)
        {
            Status s = children[currentNode].Tick();
            if (s == Status.BH_RUNNING)
                return s;
            else if (s == Status.BH_FAILURE)
            {
                currentNode = 0;
                return s;
            }
            else
            {
                currentNode++;
                if (currentNode < children.Count)
                    return Status.BH_RUNNING;
                else
                {
                    currentNode = 0;
                    return Status.BH_SUCCESS;
                }
            }
        }
        
        return Status.BH_SUCCESS;

    }

}
