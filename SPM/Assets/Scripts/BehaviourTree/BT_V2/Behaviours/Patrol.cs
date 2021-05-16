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
        Debug.Log("Patrol init");
    }
    private void SetPatrolPoint()
    {
        destination = owner.Pathfinder.GetSamplePositionOnNavMesh(owner.transform.position, patrolRadius, 10);
    }

    public override Status Evaluate()
    {
        if (Vector3.Distance(bt.ownerTransform.position, destination) < 1)
        {
            SetPatrolPoint();
            return Status.BH_SUCCESS;
        }
        else
        {
            bt.owner.Pathfinder.agent.SetDestination(destination);
            return Status.BH_RUNNING;
        }


/*
        Debug.Log("SPP eval");
        if (owner.patrolPoint != Vector3.zero)
        {
            Debug.Log("PP not null, returnerar fail");
            return Status.BH_FAILURE;
        }
        Debug.Log("S�tter PP");
        owner.patrolPoint = owner.Pathfinder.GetSamplePositionOnNavMesh(owner.transform.position, patrolRadius, 100);
        return Status.BH_SUCCESS;*/
    }



}
