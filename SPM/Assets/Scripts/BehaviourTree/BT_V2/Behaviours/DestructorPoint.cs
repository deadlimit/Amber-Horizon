using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructorPoint : BTNode
{
    public DestructorPoint(BehaviourTree bt) : base(bt) { }

    public override Status Evaluate()
    {
        bt.owner.Animator.SetTrigger("Point");
        return Status.BH_SUCCESS;
    }
}
