using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbilitySystem;

public class Shoot : BTNode
{
    private float shootCD;
    private bool animationFinished;
    private bool animationStarted;
    private Transform playerTransform;
    public Shoot(BehaviourTree bt) : base(bt) 
    {
        shootCD = bt.GetBlackBoardValue<float>("FireCooldown").GetValue();
        playerTransform = bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue();
    }
    public override void OnInitialize()
    {
        Debug.Log("Shoot init");
    }
    public override Status Evaluate()
    {
        if (bt.timerNode.GetAttackCooldown() > 0)
        {
            return Status.BH_FAILURE;
        }
       

        if (animationFinished)
        {
            //bt.ownerAgent.isStopped = false;
            animationFinished = false;
            animationStarted = false;
            bt.timerNode.SetAttackCooldown(shootCD);
            Debug.Log("Shoot finished SUCCESS");
            return Status.BH_SUCCESS;
        }

        // I guess this is the type of code you would place in OnInitialize, the problem with in this specific case of course,
        //is that shoot cd is checked inside of the node, and that would let the animation and cd start despite not actually being off cooldown
        //
        if (!animationStarted)
        {
            animationStarted = true;
            //bt.ownerAgent.ResetPath();
            bt.owner.Animator.SetTrigger("Shoot");
            bt.ownerTransform.LookAt(playerTransform);
        }
       
        Debug.Log("Shoot running..");
        return Status.BH_RUNNING;


    }

    //I would really like a sort of interface to manage the Animation events calling to  tell the tree that the animation has
    // finished, this sort of programming will quickly get difficult to handle. I guess timers would do the trick but that
    // seems even worse to me
    public void ShootAnimationFinished()
    {
        animationFinished = true;
    }
}
