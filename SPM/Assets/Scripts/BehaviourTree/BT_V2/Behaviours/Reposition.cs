using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : BTNode
{
    private float placeHolderOrigin = 1;
    private float placeHolderMaxDist = 3f;
    private Transform playerTransform;
    public Reposition(BehaviourTree bt) : base(bt) { }

    public override void OnInitialize()
    {
        playerTransform = bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue(); 
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
    //x = cx + r * cos(a)
    //y = cy + r* sin(a)
    private Vector3 CalculateNewPosition()
    {
        float randomValue = Random.Range(-1, 1f);
        float x = playerTransform.position.x + 8 - 1 * Mathf.Cos(randomValue);
        float z = playerTransform.position.z + 8 - 1 * Mathf.Sin(randomValue);
        Vector3 newPos = new Vector3(x, bt.ownerTransform.position.y, z);
        //Vector3 newPos = bt.owner.Pathfinder.GetSamplePositionOnNavMesh(bt.ownerTransform.position, placeHolderOrigin, placeHolderMaxDist);
        

        return newPos;
    }
}
