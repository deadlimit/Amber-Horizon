using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioProximityCheck : BTNode
{

    private float hearingRange = 8f;

    //Saving resources
    private Status cachedValue = Status.BH_FAILURE;
    private int frameCounter;
    private int framesBetweenOverlapCasts = 10;
    public AudioProximityCheck(BehaviourTree bt) : base(bt) {}

    public override Status Evaluate()
    {
        //No need to execute the overlap and subsequent instructions every single Update/Evaluate call,
        //So between the calls we'll simply store the last obtained value so as to not interrupt the tree structure
        frameCounter++;

        if (frameCounter % framesBetweenOverlapCasts != 0)
        {
            return cachedValue;
        }
        frameCounter = 0;



        Collider[] arr = Physics.OverlapSphere(bt.ownerTransform.position, hearingRange, bt.owner.GetPlayerMask());
        if (arr.Length > 0)
        {
            foreach (Collider coll in arr)
            {
                //This should only ever return one collision, as there's only one player.
                //If more player characters were introduced, the blackboard value "Target" becomes nonsensical
                //without determining which player to target.
                bt.GetBlackBoardValue<Vector3>("Target")?.SetValue(coll.transform.position);
                
                if(bt.blackboard["Target"] == null)
                    bt.blackboard["Target"] = new BehaviourTree.DataContainer<Vector3>(coll.transform.position);
            }
            //If someone were to forget the limitations of this code as stated above, we'll log an error
            if (arr.Length > 1)
                Debug.LogError("Arr Length > 1!!");
            
            cachedValue = Status.BH_SUCCESS;
            return Status.BH_SUCCESS;
        }
        cachedValue = Status.BH_FAILURE;
        return Status.BH_FAILURE;
    }
}
