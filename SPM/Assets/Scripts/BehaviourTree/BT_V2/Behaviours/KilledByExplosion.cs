using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledByExplosion : BTNode
{
    private float timer = 3f;
    public KilledByExplosion(BehaviourTree bt) : base(bt) { }

    public override Status Evaluate()
    {
        timer -= Time.deltaTime;
        bt.ownerAgent.enabled = false;

        if (timer <= 0)
            return Status.BH_SUCCESS;

       
        return Status.BH_RUNNING;
    }
}
