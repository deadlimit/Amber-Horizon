using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Decorator
{
    public Inverter(BTNode child, BehaviourTree bt) : base(child, bt)
    {
    }

    public override Status Evaluate()
    {
        switch (m_child.Evaluate())
        {
            case Status.BH_FAILURE:
                {
                    return Status.BH_SUCCESS;
                    
                }
            case Status.BH_SUCCESS:
                {
                    return Status.BH_FAILURE;
                }
            case Status.BH_RUNNING:
                return Status.BH_RUNNING;
        }
        //Child could not be evaluated
        return Status.BH_INVALID;
    }
}
