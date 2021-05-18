using UnityEngine;

public class VisualProximityCheck : BTNode
{
    private float visualRange = 20f;
    private Transform playerTransform;
    private Vector3 lastKnownPlayerPosition;
   public VisualProximityCheck(BehaviourTree bt ) : base(bt)
    {
    }
    public override Status Evaluate()
    {  
        //Vill troligtvis ha en offset s� att synen g�r fr�n huvudet? 
        Collider[] arr = Physics.OverlapSphere(bt.ownerTransform.position, visualRange, bt.GetPlayerMask());
        if (arr.Length > 0)
        {
            foreach (Collider coll in arr)
            {
                //foreach, men vi borde bara f� ut en collider
                //m�ste ocks� h�r best�mma om target �r ett GO eller en position... lutar �t position.
                playerTransform = coll.transform;
            }
            if (arr.Length > 1)
                Debug.Log("Arr Length > 1!!");

            //Ska inte kunna hamna h�r utan att playerTransform f�r ett v�rde, men det �r inte direkt idiots�kert heller
            //kolla in bitskift- grejen f�r lager, tack, h�rdkodningen �r ju fruktansv�rd
            if (CheckLineOfSight())
                return Status.BH_FAILURE;
           
            Debug.Log("AI Detection Visual"); 
            bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(playerTransform);
            lastKnownPlayerPosition = playerTransform.position;
            Debug.Log("LastKnownPosition: " + lastKnownPlayerPosition);
            return Status.BH_SUCCESS;
        }
        //Vi s�tter inte "Target" om spelaren g�r bakom en v�gg, och koden fr�n Linecast dupliceras h�r
        //bt.GetBlackBoardValue<Vector3>("LastSeenPosition").SetValue(lastKnownPlayerPosition);
        bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(null);
        Debug.Log("VisualProxCheck Returning Failure-----------------");
        return Status.BH_FAILURE;
        Debug.Log("After return failure");
      
        //kolla riktning? 
    }

    private bool CheckLineOfSight()
    {
        if (Physics.Linecast(bt.ownerTransform.position, playerTransform.position, out var hitInfo, (1 << bt.GetPlayerMask())))
        {
            //Om vi tr�ffar n�got som inte �r spelaren, s� �r siktlinjen bruten -> 
            Debug.Log(hitInfo.collider.gameObject);
            bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(null);
            return true;
        }
        return false;
    }
}
