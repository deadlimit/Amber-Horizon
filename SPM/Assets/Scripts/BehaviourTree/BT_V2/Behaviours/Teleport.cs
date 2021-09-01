using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : BTNode
{
    //Teleport parameters
    private Vector3 teleportPosition;
    private float distanceFromPlayer = 10f;
    private Transform playerTransform;

    //checks for animations
    private bool animationStarted;
    private bool teleportFinished;
    public Teleport(BehaviourTree bt) : base(bt) { }

    public override Status Evaluate()
    {
        if (!animationStarted)
        {
            animationStarted = true;
            bt.owner.Animator.StopPlayback();
            bt.owner.Animator.SetTrigger("Teleport");
            CalculateTeleportPosition();
        }

        if(teleportFinished)
        {
            bt.ownerTransform.LookAt(playerTransform);
            teleportFinished = false;
            return Status.BH_SUCCESS;
        }
        else
        {
            return Status.BH_RUNNING;
        }
    }

    //TODO; Can result in a position so close to the player that teleport is immediately called again, fix required
    private void CalculateTeleportPosition()
    {
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
