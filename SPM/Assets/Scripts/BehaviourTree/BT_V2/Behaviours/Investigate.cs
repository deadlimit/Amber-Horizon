using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investigate : BTNode
{
    private Vector3 targetPos;
    int frameCounter = 0; 
    public Investigate(BehaviourTree bt) : base(bt)
    {
    }

    public override void OnInitialize()
    {
        Debug.Log("Investigate Init");
        //Dessa rader kan f�renklas om vi inte vill kolla tv� olika blackboardv�rden h�r (inkl. LastSeenPosition is�fall) 
        //Os�ker h�mtning och skum kastning, hur st�dar vi detta? 
        BehaviourTree.DataContainer<Vector3> targetContainer = bt.GetBlackBoardValue<Vector3>("Target");
        targetPos = targetContainer.GetValue();

        bt.owner.Pathfinder.agent.SetDestination(targetPos);
    }
    //bygger p� att target inte �r null
    //h�rdkodat v�rde, var vill vi placera det? samma sak i Patrol-scriptet
    //h�r nullar vi DataContainer
    public override Status Evaluate()
    {

        if (ReachedTarget())
        {
            Debug.Log("Blackboard 'Target' set to null ");
            bt.blackboard["Target"] = null;
            return Status.BH_SUCCESS;
        }
        //Tar vi oss hit har det f�rsta if-statementet redan k�rts
        else if (bt.blackboard["Target"] == null)
        {
            return Status.BH_FAILURE;
        }

        else
            Debug.Log("Investigate running");
            return Status.BH_RUNNING;
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
    private void FrameCountDebug()
    {
        if (frameCounter % 10 == 0)
        {
            Debug.Log("Investigating");
            frameCounter = 0;
        }
        frameCounter++;

    }
}
