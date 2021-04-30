using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPatrolPoint : BTNode
{
    Forager owner;
    public MoveToPatrolPoint(object owner, BehaviourTree bt) : base(bt)
    {
        Debug.Log("MTPP Skapad");
        this.owner = (Forager)owner;
    }
    public override void OnInitialize()
    {
        Debug.Log("MTPP OnInit");
        owner.Pathfinder.agent.Move(owner.patrolPoint * Time.deltaTime );
    }
    public override Status Evaluate()
    {
        Debug.Log("MTPP eval");
        //Om vi är framme, return success
        if (Vector3.Distance(owner.transform.position, owner.patrolPoint) < 1)
        {
            owner.patrolPoint = Vector3.zero;
            return Status.BH_SUCCESS;
        }
        if (owner.patrolPoint == null)
            return Status.BH_FAILURE;
        if (owner.patrolPoint != null)
            return Status.BH_RUNNING;

        return Status.BH_INVALID;

    }


}
