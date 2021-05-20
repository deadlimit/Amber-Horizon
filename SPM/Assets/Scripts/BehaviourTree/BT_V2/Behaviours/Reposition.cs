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
        //S�tt reposition-position.. p� n�got s�tt
        bt.ownerAgent.SetDestination(CalculateNewPosition());
        base.OnInitialize();
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

    private Vector3 CalculateNewPosition()
    {
        Vector3 newPos = bt.owner.Pathfinder.GetSamplePositionOnNavMesh(bt.ownerTransform.position, placeHolderOrigin, placeHolderMaxDist); 

        //t�nkte skriva en egen metod h�r, eventuellt en som f�ljer omkretsen av en cirkel
        //som utg�r fr�n spelaren, med forager range som diameter. Men trigonometri suger.

        return newPos;
    }
}
