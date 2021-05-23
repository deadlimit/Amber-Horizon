using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertAllies : BTNode
{
    Transform playerTransform;
    public AlertAllies(BehaviourTree bt) : base(bt) { }

    public override Status Evaluate()
    {
        if (bt.GetBlackBoardValue<bool>("HasCalledForHelp").GetValue())
            return Status.BH_FAILURE;

        Debug.Log("Alerting allies!");
        AlertNearby();
        bt.GetBlackBoardValue<bool>("HasCalledForHelp").SetValue(true);
        playerTransform = bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue();
        return Status.BH_SUCCESS;
    }

    private void AlertNearby()
    {
        Collider[] arr = Physics.OverlapSphere(bt.ownerTransform.position, bt.owner.outerRing, bt.owner.EnemyMask);
        if (arr.Length > 0)
        {
            foreach (Collider coll in arr)
            {
                coll.gameObject.GetComponent<Forager>().Alert(playerTransform);
            }
        }
    }
}
