using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KilledByBlackHole : BTNode
{
    private float threshold = 0.05f;
    private float lerpSpeed = 0.8f;
    private Transform bhTransform; 
    public KilledByBlackHole(BehaviourTree bt) : base(bt) { }

    public override Status Evaluate()
    {
        if (!bt.owner.hitByBlackHole)
            return Status.BH_FAILURE;
        
        if (!bhTransform)
            bhTransform = bt.owner.activeBlackHole.transform;

        bt.owner.Animator.SetBool("Die", true);
        bt.ownerAgent.ResetPath();
        bt.ownerTransform.LookAt(bhTransform.transform);
        bt.owner.transform.position = Vector3.Lerp(bt.ownerTransform.position, bhTransform.position, Time.deltaTime * lerpSpeed);
        bt.ownerTransform.localScale = Vector3.Lerp(bt.ownerTransform.localScale, Vector3.zero, Time.deltaTime * lerpSpeed);

        if(bt.ownerTransform.localScale.magnitude < threshold )
            return Status.BH_SUCCESS;

        return Status.BH_RUNNING;
    }
}
