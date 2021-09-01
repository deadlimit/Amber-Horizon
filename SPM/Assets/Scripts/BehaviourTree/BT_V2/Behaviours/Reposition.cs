using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : BTNode
{

    private Transform playerTransform;
    private float maxAngle = 0f; 

    //Reference only needed to set maxAngle from the inspector
    private BTForager foragerBT;

    public Reposition(BehaviourTree bt) : base(bt) 
    {
        foragerBT = (BTForager)bt;
        Debug.Assert(foragerBT);
        maxAngle = foragerBT.forager.MaxRepositionAngle;
    }

    public override void OnInitialize()
    {
        bt.ownerAgent.enabled = true;
        bt.ownerAgent.ResetPath();
        playerTransform = bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue();
        bt.ownerAgent.SetDestination(CalculateNewPosition());
    }
    public override Status Evaluate()
    {
        //This check is needed because the tree is practically locked while the AI is trying to fire,
        //and if the Fire fails after some time, the player may have moved out of range already.
        //this is an obvious flaw in how the tree is built right now, since TargetInRange already checks the distance
        if (ReachedTarget())
        {
            return Status.BH_SUCCESS;
        }

        else
        {
            bt.ownerTransform.LookAt(playerTransform);
            return Status.BH_RUNNING;
        }
    }

    private bool ReachedTarget()
    {
        if (!bt.owner.Pathfinder.agent.pathPending)
        {
            if (bt.owner.Pathfinder.agent.remainingDistance <= bt.owner.Pathfinder.agent.stoppingDistance)
            {
                if (!bt.owner.Pathfinder.agent.hasPath || bt.owner.Pathfinder.agent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private Vector3 CalculateNewPosition()
    {
        float angle = Random.Range(-maxAngle, maxAngle);

         Vector3 direction = bt.ownerTransform.position - playerTransform.position;
         Vector3 modifiedDirection = Quaternion.AngleAxis(angle, Vector3.up) * direction;
      
        return modifiedDirection + playerTransform.position;

    }
}
