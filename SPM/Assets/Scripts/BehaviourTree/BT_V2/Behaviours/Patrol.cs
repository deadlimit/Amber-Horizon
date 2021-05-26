using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : BTNode
{
    //No target, vad gör vi då, och vad klassas som att "Lyckas"? 
    public float speed = 5f;
    protected Vector3 destination;
    private float patrolRadius = 5f;
    public Patrol(BehaviourTree bt) : base(bt)
    {
        destination = Vector3.zero;
    }

    public override void OnInitialize()
    {
        bt.ownerAgent.speed = bt.owner.MovementSpeedDefault;
    }
    public override Status Evaluate()
    {
        if (ReachedTarget() || destination.Equals(Vector3.zero))
        {
            SetPatrolPoint();
            return Status.BH_SUCCESS;
        }
        else
        {
            return Status.BH_RUNNING;
        }
    }
    private void SetPatrolPoint()
    {
        destination = bt.owner.Pathfinder.GetSamplePositionOnNavMesh(bt.startPos, patrolRadius, 10);
        bt.ownerAgent.SetDestination(destination);
    }

    //Den här metoden behövs i alla metoder som utvärderar om navAgent är framme
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

}
