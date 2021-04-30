using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : BTNode
{
    //No target, vad gör vi då, och vad klassas som att "Lyckas"? 
    Forager owner;
    public float speed = 5f;
    protected Vector3 destination;
    private float patrolRadius = 5f;
    private BehaviourTree m_tree;
    public Patrol(object owner, BehaviourTree bt) : base(bt)
    {
        m_tree = bt;
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
        //lite noggrannare checks här, definitivt.
        destination = owner.Pathfinder.GetSamplePositionOnNavMesh(owner.transform.position, patrolRadius, 10);
        Debug.Log("Destination: " + destination);
    }

    public override Status Evaluate()
    {
        if (Vector3.Distance(m_tree.gameObject.transform.position, destination) < 1)
        {
            SetPatrolPoint();
            return Status.BH_SUCCESS;
        }
        else
        {
            m_tree.owner.Pathfinder.agent.SetDestination(destination);
            return Status.BH_RUNNING;
        }


/*
        Debug.Log("SPP eval");
        if (owner.patrolPoint != Vector3.zero)
        {
            Debug.Log("PP not null, returnerar fail");
            return Status.BH_FAILURE;
        }
        Debug.Log("Sätter PP");
        owner.patrolPoint = owner.Pathfinder.GetSamplePositionOnNavMesh(owner.transform.position, patrolRadius, 100);
        return Status.BH_SUCCESS;*/
    }



}
