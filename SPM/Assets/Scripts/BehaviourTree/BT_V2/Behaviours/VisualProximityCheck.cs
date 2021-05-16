using UnityEngine;

public class VisualProximityCheck : BTNode
{
    private float visualRange = 10f;
   public VisualProximityCheck(BehaviourTree bt ) : base(bt)
    {
    }
    public override Status Evaluate()
    {  
        //kasta overlap
        Collider[] arr = Physics.OverlapSphere(bt.ownerTransform.position, visualRange, bt.GetPlayerMask());
        if (arr.Length > 0)
        {
            Debug.Log("AI Detection Visual");
            foreach (Collider coll in arr)
            {
                //foreach, men vi borde bara få ut en collider
                //måste också här bestämma om target är ett GO eller en position... lutar åt position. 
                bt.blackboard["Target"] = coll.transform.position;
            }
            if (arr.Length > 1)
                Debug.Log("Arr Length > 1!!");

            return Status.BH_SUCCESS;
        }

        return Status.BH_FAILURE;
      
        //kolla riktning? 
        //linecast? 
    }
}
