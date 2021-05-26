using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetValuesAfterVisual_Destructor : BTNode
{

    public SetValuesAfterVisual_Destructor(BehaviourTree bt) : base(bt) { }

    public override Status Evaluate()
    {
        //Destructor specific animator parameter, which conceptually belongs in VisualProximityCheck, but that script is shared by both enemies
        bt.owner.Animator.SetBool("CanSeePlayer", true);
        bt.owner.Animator.SetBool("PlayerInRange", false);
        bt.owner.Animator.SetBool("HasPositionToInvestigate", true);

        //Return value dependent on which type of node parents this one, right now the parent is a selector
        //and we want this node to execute without interrupting the BT execution, hence return FAILURE
        return Status.BH_FAILURE;
    }
}
