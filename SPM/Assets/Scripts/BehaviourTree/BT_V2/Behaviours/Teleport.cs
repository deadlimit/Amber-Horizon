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

    //Egentligen vill vi se till att positionen som räknas ut inte riskerar att befinna sig för nära spelaren
    //det är inte bara korkat, det blir krångligare att styra beteendet då också

    private void CalculateTeleportPosition()
    {
        playerTransform = bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue();

        teleportPosition = playerTransform.position + new Vector3(
            Random.Range(-distanceFromPlayer, distanceFromPlayer),
            Random.Range(-distanceFromPlayer, distanceFromPlayer),
            Random.Range(-distanceFromPlayer, distanceFromPlayer));
    }
    public void ExecuteTeleport()
    {
        bt.owner.transform.position = teleportPosition;
        bt.ownerTransform.LookAt(playerTransform);
        bt.ownerAgent.ResetPath();
        animationStarted = false;
        teleportFinished = true;
    }
}
