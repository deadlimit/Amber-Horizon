using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbilitySystem;

public class Shoot : BTNode
{
    private float shootCD;
    private bool animationFinished;
    private bool animationStarted;
    public Shoot(BehaviourTree bt) : base(bt) 
    {
        shootCD = bt.GetBlackBoardValue<float>("FireCooldown").GetValue();
    }

    public override Status Evaluate()
    {
        if (bt.timerNode.GetAttackCooldown() > 0)
        {
            return Status.BH_FAILURE;
        }
        bt.ownerAgent.ResetPath();

        if (animationFinished)
        {
            animationFinished = false;
            animationStarted = false;
            bt.ownerAgent.enabled = true; 

            bt.timerNode.SetAttackCooldown(shootCD);
            return Status.BH_SUCCESS;
        }

        // I guess this is the type of code you would place in OnInitialize, the problem with in this specific case of course,
        //is that shoot cd is checked inside of the node, and that would let the animation and cd start despite not actually being off cooldown
        if (!animationStarted)
        {
            animationStarted = true;
            LockAgent();
            bt.owner.Animator.SetTrigger("Shoot");
        }
       
        return Status.BH_RUNNING;


    }

    //I would really like a sort of interface to manage the Animation events calling to  tell the tree that the animation has
    // finished, this sort of programming will quickly get difficult to handle. I guess timers would do the trick but that
    // seems even more bloated to me
    public void ShootAnimationFinished()
    {
        animationFinished = true;
    }

    private void LockAgent()
    {

        //agent is enabled again in TargetInRange if the shoot animation can not finish properly - 
        //and again inside Teleport's ExecuteTeleport, in case the player triggers this behaviour, interrupting the shoot-animation
        bt.ownerAgent.ResetPath();
        bt.ownerAgent.enabled = false;

        //Seems like temporarily disabling the agent is the only thing actually needed, will leave the other 
        //lines in for now, so it'll be easier to remember this stuff if a new problem arises
        /*bt.owner.Animator.StopPlayback();
        bt.ownerTransform.LookAt(playerTransform);*/
    }
}
