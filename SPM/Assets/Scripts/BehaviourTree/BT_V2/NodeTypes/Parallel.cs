using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Evaluates all children regardless of order and/or status they return.
 * Currentl version is only made to handle running a timer simultaneous to the behaviour tree.
 */
public class Parallel : Composite
{
    //Constructors
    public Parallel(List<BTNode> children, BehaviourTree bt) : base(children, bt) {    }
    public Parallel(List<BTNode> children, BehaviourTree bt, string name) : base(children, bt, name) { }

    public override Status Evaluate()
    {
        foreach(BTNode child in children)
        {
            child.Tick();
        }
        //I only use the parallel as a direct child of Repeater, and as such the return value doesn't really matter.
        return Status.BH_SUCCESS;
    }
}
