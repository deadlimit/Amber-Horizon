using UnityEngine;

public class VisualProximityCheck : BTNode
{
    private float visualRange = 20f;
    private Transform playerTransform;
   public VisualProximityCheck(BehaviourTree bt ) : base(bt)
    {
    }
    public override Status Evaluate()
    {  
        //Vill troligtvis ha en offset så att synen går från huvudet? 
        Collider[] arr = Physics.OverlapSphere(bt.ownerTransform.position, visualRange, bt.GetPlayerMask());
        if (arr.Length > 0)
        {
            foreach (Collider coll in arr)
            {
                //foreach, men vi borde bara få ut en collider
                //måste också här bestämma om target är ett GO eller en position... lutar åt position.
                playerTransform = coll.transform;
            }
            if (arr.Length > 1)
                Debug.Log("Arr Length > 1!!");

            //Ska inte kunna hamna här utan att playerTransform får ett värde, men det är inte direkt idiotsäkert heller
            //kolla in bitskift- grejen för lager, tack, hårdkodningen är ju fruktansvärd
            if (Physics.Linecast(bt.ownerTransform.position, playerTransform.position, out var hitInfo, (1 << bt.GetPlayerMask())))
            {
                //Om vi träffar något som inte är spelaren, så är siktlinjen bruten -> 
                Debug.Log(hitInfo.collider.gameObject);
                return Status.BH_FAILURE;
            }
            Debug.Log("AI Detection Visual"); 
            bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(playerTransform);
            return Status.BH_SUCCESS;
        }

        return Status.BH_FAILURE;
      
        //kolla riktning? 
    }
}
