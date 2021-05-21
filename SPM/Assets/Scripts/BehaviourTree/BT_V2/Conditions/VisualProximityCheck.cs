using UnityEngine;

public class VisualProximityCheck : BTNode
{
    private float visualRange = 20f;
    private Transform playerTransform;
    private Vector3 lastKnownPlayerPosition;
    private bool hasSeenPlayer; 
   public VisualProximityCheck(BehaviourTree bt ) : base(bt)
    {
    }

    public override Status Evaluate()
    {  
        //Vill troligtvis ha en offset s� att synen g�r fr�n huvudet? 
        Collider[] arr = Physics.OverlapSphere(bt.ownerTransform.position, visualRange, bt.owner.GetPlayerMask());
        if (arr.Length > 0)
        {
            hasSeenPlayer = true;

            foreach (Collider coll in arr)
            {
                playerTransform = coll.transform;
            }
            if (arr.Length > 1)
                Debug.LogError("Player Collider Array Length > 1!!");

          
            if (!PlayerInLineOfSight() || !PlayerInAngleOfSight())
                return Status.BH_FAILURE;

            //if(angle)

            Debug.Log("AI Detection Visual");
            ResetPatrolPoint();
            bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(playerTransform);
            //Tidigare har set destination bara kallats i moveToTarget, men det kan ta en stund innan den n�r dit..? 
            lastKnownPlayerPosition = playerTransform.position;
            Debug.Log("LKPP: " + lastKnownPlayerPosition);
            return Status.BH_SUCCESS;
        }
        else
        {
            SetLastSeenPosition();

            //Vi s�tter inte "Target" om spelaren g�r bakom en v�gg, och koden fr�n Linecast dupliceras h�r
            bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(null);
            Debug.Log("VisualProxCheck Returning Failure-----------------,");
            return Status.BH_FAILURE;
        }
        //kolla riktning? 
    }

    private bool PlayerInLineOfSight()
    {
        if (Physics.Linecast(bt.ownerTransform.position, playerTransform.position, out var hitInfo, (1 << bt.owner.GetPlayerMask())))
        {
            //Om vi tr�ffar n�got som inte �r spelaren, s� �r siktlinjen bruten -> 
            bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(null);
            return false;
        }
        return true;
    }

    //Beh�ver inte st�ndigt uppdatera blackboardv�rdet, reserverar det till LastKnownPosition blir relevant, 
    //allts� n�r spelaren l�mnar vision, default value �r Vector3.zero
    private void SetLastSeenPosition()
    {
        if (hasSeenPlayer)
        {
            bt.GetBlackBoardValue<Vector3>("LastSeenPosition").SetValue(lastKnownPlayerPosition);
            Debug.Log("LastSeenPosition Set to: " + lastKnownPlayerPosition);
            Debug.Log("Setting last seen position" + lastKnownPlayerPosition);
            lastKnownPlayerPosition = Vector3.zero;
            hasSeenPlayer = false;
        }
    }
    private void ResetPatrolPoint()
    {
        bt.blackboard["Target"] = null;
    }

    private bool PlayerInAngleOfSight()
    {
        Vector3 forward = bt.ownerTransform.TransformDirection(Vector3.forward);
        Vector3 angle = playerTransform.position - bt.ownerTransform.position;
        if (Vector3.Dot(forward, angle) > 0)
            return true;
        return false;
    }
}
