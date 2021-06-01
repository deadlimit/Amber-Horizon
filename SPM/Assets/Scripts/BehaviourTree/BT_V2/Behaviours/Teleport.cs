using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : BTNode
{

    Vector3 teleportPosition;
    private float distanceFromPlayer = 10f;
    private bool animationStarted;
    private bool teleportFinished;
    private Transform playerTransform;
    public Teleport(BehaviourTree bt) : base(bt) { }

    public override Status Evaluate()
    {
        if (!animationStarted)
        {
            Debug.Log("Teleport Animation Started");
            animationStarted = true;
            bt.owner.Animator.StopPlayback();
            bt.owner.Animator.SetTrigger("Teleport");
            CalculateTeleportPosition();
        }

        if(teleportFinished)
        {
            Debug.Log("Teleport Finished");
            bt.ownerTransform.LookAt(playerTransform);
            teleportFinished = false;
            return Status.BH_SUCCESS;
        }
        else
        {
            return Status.BH_RUNNING;
        }
    }


    //Somehow we would like to make sure to teleport position is not too close to the player, because this results in another teleport
    //immediately, and that feels pretty dumb
    private void CalculateTeleportPosition()
    {
        //GetSamplePositionOnNavMesh would work except for the fact that we want to base the position
        //on the players location, and that method (right now) does not take a transform argument
        //the problem now is that y-position could end up NOT being on the navmesh
        playerTransform = bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue();

        teleportPosition = playerTransform.position + new Vector3(
            Random.Range(-distanceFromPlayer, distanceFromPlayer),
            bt.ownerTransform.position.y,
            Random.Range(-distanceFromPlayer, distanceFromPlayer));
    }
    public void ExecuteTeleport()
    {
        bt.ownerAgent.enabled = true;

        bt.owner.transform.position = teleportPosition;
        bt.ownerTransform.LookAt(playerTransform);
        bt.ownerAgent.ResetPath();
        animationStarted = false;
        teleportFinished = true;
    }
}
