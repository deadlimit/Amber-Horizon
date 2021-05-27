using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : BTNode
{
    private float placeHolderOrigin = 1;
    private float placeHolderMaxDist = 3f; 
    public Reposition(BehaviourTree bt) : base(bt) { }

    public override void OnInitialize()
    {
        bt.ownerAgent.SetDestination(CalculateNewPosition());
    }
    public override Status Evaluate()
    {
        if (ReachedTarget())
            return Status.BH_SUCCESS;
        else
        {
            bt.ownerTransform.LookAt(bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue());
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

    //Preferably this position is calculated on the circumference of a circle, but thats not quick maths
    // i have to learn trigonometry first
    private Vector3 CalculateNewPosition()
    {
        Vector3 newPos = bt.owner.Pathfinder.GetSamplePositionOnNavMesh(bt.ownerTransform.position, placeHolderOrigin, placeHolderMaxDist); 

        return newPos;
    }
}
