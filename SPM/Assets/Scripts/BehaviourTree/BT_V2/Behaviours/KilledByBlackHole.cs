using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledByBlackHole : BTNode
{
    //NOTE!
    //Forager specific node, casting to BTForager and accessing forager variables
    //
    private float threshold = 0.05f;
    private float lerpSpeed = 0.8f;
    private Transform bhTransform;
    private BTForager foragerBT;
    public KilledByBlackHole(BehaviourTree bt) : base(bt) 
    {
        foragerBT = (BTForager)bt;
        Debug.Assert(foragerBT);
    }

    public override Status Evaluate()
    {
        if (!foragerBT.forager.hitByBlackHole)
            return Status.BH_FAILURE;

        if (!bhTransform)
        {
            StartDeath();
            bhTransform = foragerBT.forager.activeBlackHole.transform;
        }

        bt.ownerTransform.LookAt(bhTransform.transform);
        bt.owner.transform.position = Vector3.Lerp(bt.ownerTransform.position, bhTransform.position, Time.deltaTime * lerpSpeed);
        bt.ownerTransform.localScale = Vector3.Lerp(bt.ownerTransform.localScale, Vector3.zero, Time.deltaTime * lerpSpeed);

        if(bt.ownerTransform.localScale.magnitude < threshold )
            return Status.BH_SUCCESS;

        return Status.BH_RUNNING;
    }

    private void StartDeath()
    {
        bt.ownerAgent.enabled = false;
        bt.owner.Animator.SetBool("Die", true);
    }
}
