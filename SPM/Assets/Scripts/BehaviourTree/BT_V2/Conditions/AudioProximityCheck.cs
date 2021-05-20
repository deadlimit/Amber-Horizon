using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioProximityCheck : BTNode
{
    private float hearingRange = 0f;
    public AudioProximityCheck(BehaviourTree bt) : base(bt) {}
    public override void OnInitialize()
    {

        Debug.Log("AudioProximityCheck"); 
    }
    public override Status Evaluate()
    {
        Collider[] arr = Physics.OverlapSphere(bt.ownerTransform.position, hearingRange, bt.GetPlayerMask());
        if (arr.Length > 0)
        {
            Debug.Log("AI Detection Audio");
            foreach (Collider coll in arr)
            {
                //foreach, men vi borde bara f� ut en collider
                //DataContainer "Target" �r nullad, s� h�r m�ste vi is�fall skapa en ny om den inte existerar (vilket den troligtvis inte g�r) 
                //det �r skit men fungerar f�r nu
                bt.GetBlackBoardValue<Vector3>("Target")?.SetValue(coll.transform.position);
                
                if(bt.blackboard["Target"] == null)
                    bt.blackboard["Target"] = new BehaviourTree.DataContainer<Vector3>(coll.transform.position);
            }
            if (arr.Length > 1)
                Debug.Log("Arr Length > 1!!");

            return Status.BH_SUCCESS;
        }
        Debug.Log("Failure from audioProxCheck");
        return Status.BH_FAILURE;
    }
}
