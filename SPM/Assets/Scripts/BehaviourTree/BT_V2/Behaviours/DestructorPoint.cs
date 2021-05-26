using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructorPoint : BTNode
{
    private bool animationFinished;
    public DestructorPoint(BehaviourTree bt) : base(bt) { }

    public override Status Evaluate()
    {
        if (animationFinished)
            return Status.BH_SUCCESS;

        //Only want the point to happen once per engagement, and to be skipped if Attack OR Point already has been executed
        // The question is where to reset this value, a Get/Set value in a generic node would result in error from Forager BT      
        if (bt.GetBlackBoardValue<bool>("HasAlreadyPointed").GetValue())
            return Status.BH_FAILURE;

        else
        {
            bt.ownerTransform.LookAt(bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue());
            bt.ownerAgent.ResetPath();
            bt.owner.Animator.SetTrigger("Point");
            bt.GetBlackBoardValue<bool>("HasAlreadyPointed").SetValue(true);
            return Status.BH_RUNNING;
        }
    }

    //Called by animation event through behaviour tree
    public void PointAnimationFinished()
    {
        animationFinished = true;
    }
}
