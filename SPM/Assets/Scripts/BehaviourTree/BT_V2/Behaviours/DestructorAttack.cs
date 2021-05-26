using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbilitySystem;

public class DestructorAttack : BTNode
{
    private bool animationStarted;
    private bool animationFinished;
    public DestructorAttack(BehaviourTree bt) : base(bt)
    {

    }
    //TODO; Correct evaluate method which can return failure, and possibly even running. right now the attack cannot fail
    public override void OnInitialize()
    {
        //Reset local bool, and tell Animator "PlayerInRange", this animator parameter controls attack animation
        animationFinished = false;
        bt.owner.Animator.SetBool("PlayerInRange", true);

    }
    public override Status Evaluate()
    {
        if (animationFinished)
            return Status.BH_SUCCESS;

        if (!animationStarted && bt.owner.AbilitySystem.TryActivateAbilityByTag(GameplayTags.MeleeTag))
        {
            bt.owner.Pathfinder.agent.ResetPath();
            bt.owner.Animator.SetTrigger("Melee");
            animationStarted = true;
            return Status.BH_RUNNING;
        }
        else
            return Status.BH_FAILURE;
        

    }

    //Called by animation event through method in BT
    public void AttackAnimationFinished()
    {
        animationStarted = false; 
    }
}
