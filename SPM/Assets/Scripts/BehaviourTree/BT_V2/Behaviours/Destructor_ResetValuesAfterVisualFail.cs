using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructor_ResetValuesAfterVisualFail : BTNode
{
    public Destructor_ResetValuesAfterVisualFail(BehaviourTree bt) : base(bt) { }

    public override Status Evaluate()
    {
        bt.owner.Animator.SetBool("CanSeePlayer", false);
        bt.GetBlackBoardValue<bool>("HasAlreadyPointed").SetValue(false);

        //Return value dependent on which type of node parents this one, right now the parent is a selector
        //and we want this node to execute without interrupting the BT execution, hence return FAILURE
        return Status.BH_FAILURE;
    }
}
