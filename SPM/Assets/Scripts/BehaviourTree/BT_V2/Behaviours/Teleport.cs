using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : BTNode
{
    //Var ska variabler som den här lagras? Man vill ha tillgång i inspectorn förstås, men det blir lite märkligt
    //att fylla på med 100 variabler i Forager själv.
    Vector3 teleportPosition;
    private float distanceFromPlayer = 10f;
    private float timeBeforeTeleport = .2f;
    private float timer;
    private bool animationStarted;
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

        if(timer <= 0)
        {
            //Vill nog att ett animationsevent styr det här, inte en timer, det kommer bli mycket mer exakt
            //isåfall en metod som sätter dessa saker och kanske en extra bool som styr returen av Status
            Debug.Log("Teleport Finished");
            bt.owner.transform.position = teleportPosition;
            animationStarted = false;
            timer = timeBeforeTeleport;
            return Status.BH_SUCCESS;
        }
        else
        {
            timer -= Time.deltaTime;
            return Status.BH_RUNNING;
        }
    }

    //Egentligen vill vi se till att positionen som räknas ut inte riskerar att befinna sig för nära spelaren
    //det är inte bara korkat, det blir krångligare att styra beteendet då också

    private void CalculateTeleportPosition()
    {
        teleportPosition = bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue().position + new Vector3(
            Random.Range(-distanceFromPlayer, distanceFromPlayer),
            Random.Range(-distanceFromPlayer, distanceFromPlayer),
            Random.Range(-distanceFromPlayer, distanceFromPlayer));
    }
}
