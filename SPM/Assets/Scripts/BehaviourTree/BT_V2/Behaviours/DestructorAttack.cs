using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbilitySystem;

public class DestructorAttack : BTNode
{
    private bool animationStarted;
    public DestructorAttack(BehaviourTree bt) : base(bt)
    {

    }
    //TODO; Correct evaluate method which can return failure, and possibly even running. right now the attack cannot fail
    public override void OnInitialize()
    {
      

    }
    public override Status Evaluate()
    {
        if (!animationStarted && bt.owner.AbilitySystem.TryActivateAbilityByTag(GameplayTags.MeleeTag))
        {
            bt.owner.Pathfinder.agent.ResetPath();
            bt.owner.Animator.SetTrigger("Melee");
            bt.owner.Animator.SetBool("PlayerInRange", true);
            animationStarted = true;

            return Status.BH_SUCCESS;
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
