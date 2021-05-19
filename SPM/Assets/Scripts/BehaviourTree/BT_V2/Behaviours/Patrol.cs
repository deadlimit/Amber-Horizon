using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : BTNode
{
    //No target, vad g�r vi d�, och vad klassas som att "Lyckas"? 
    Forager owner;
    public float speed = 5f;
    protected Vector3 destination;
    private float patrolRadius = 5f;
    public Patrol(object owner, BehaviourTree bt) : base(bt)
    {
        destination = Vector3.zero;
        this.owner = (Forager)owner;
        SetPatrolPoint();
    }
    public override void OnInitialize()
    {
        destination = Vector3.zero;
        Debug.Log("Patrol init");
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
        destination = owner.Pathfinder.GetSamplePositionOnNavMesh(bt.startPos, patrolRadius, 10);
        bt.ownerAgent.SetDestination(destination);
    }

    //Den h�r metoden beh�vs i alla metoder som utv�rderar om navAgent �r framme
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
