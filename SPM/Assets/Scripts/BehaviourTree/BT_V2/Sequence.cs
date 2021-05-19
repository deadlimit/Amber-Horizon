using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Composite
{ 
    private int currentNode = 0;
    public Sequence(List<BTNode> children, BehaviourTree bt) : base(children, bt) { }
    public Sequence(List<BTNode> children, BehaviourTree bt, string name) : base(children, bt, name) { }
    public override Status Evaluate()
    {
        //titta igenom varje barn om status på ett är RUNNING eller FAILURE, returnera det
        if(currentNode < children.Count)
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


/*
        foreach(BTNode child in children)
        {
            Status s = child.Tick();
            if (s != Status.BH_SUCCESS)
                return s;
            if (children.IndexOf(child) == children.Count - 1)
            {
                Debug.Log("All children succeeded");
                return Status.BH_SUCCESS;
            }
        }

        return Status.BH_INVALID; //Unexpected loop exit*/
    }

}
