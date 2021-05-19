using UnityEngine;

public class VisualProximityCheck : BTNode
{
    private float visualRange = 20f;
    private Transform playerTransform;
    private Vector3 lastKnownPlayerPosition = Vector3.zero;
    private bool hasSeenPlayer; 
   public VisualProximityCheck(BehaviourTree bt ) : base(bt)
    {
    }

    public override Status Evaluate()
    {  
        //Vill troligtvis ha en offset så att synen går från huvudet? 
        Collider[] arr = Physics.OverlapSphere(bt.ownerTransform.position, visualRange, bt.GetPlayerMask());
        if (arr.Length > 0)
        {
            hasSeenPlayer = true;

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
            if (CheckLineOfSight())
                return Status.BH_FAILURE;
           
            Debug.Log("AI Detection Visual"); 
            bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(playerTransform);
            //Tidigare har set destination bara kallats i moveToTarget, men det kan ta en stund innan den når dit..? 
            lastKnownPlayerPosition = playerTransform.position;
            return Status.BH_SUCCESS;
        }
       
        SetLastSeenPosition();



        //Vi sätter inte "Target" om spelaren går bakom en vägg, och koden från Linecast dupliceras här
        //bt.GetBlackBoardValue<Vector3>("LastSeenPosition").SetValue(lastKnownPlayerPosition);
        bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(null);
        Debug.Log("VisualProxCheck Returning Failure-----------------");
        return Status.BH_FAILURE;
      
        //kolla riktning? 
    }

    private bool CheckLineOfSight()
    {
        if (Physics.Linecast(bt.ownerTransform.position, playerTransform.position, out var hitInfo, (1 << bt.GetPlayerMask())))
        {
            //Om vi träffar något som inte är spelaren, så är siktlinjen bruten -> 
            Debug.Log(hitInfo.collider.gameObject);
            bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(null);
            return true;
        }
        return false;
    }

    //Behöver inte ständigt uppdatera blackboardvärdet, reserverar det till LastKnownPosition blir relevant, 
    //alltså när spelaren lämnar vision, default value är Vector3.zero
    private void SetLastSeenPosition()
    {
        if (hasSeenPlayer)
        {
            bt.GetBlackBoardValue<Vector3>("LastSeenPosition").SetValue(lastKnownPlayerPosition);
            Debug.Log("LastKnownPosition Set to: " + lastKnownPlayerPosition);
            lastKnownPlayerPosition = Vector3.zero;
            hasSeenPlayer = false;
        }
    }
}
