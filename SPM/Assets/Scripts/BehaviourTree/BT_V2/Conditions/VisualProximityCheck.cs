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
        //Vill troligtvis ha en offset så att synen går från huvudet? 
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
            //Tidigare har set destination bara kallats i moveToTarget, men det kan ta en stund innan den når dit..? 
            lastKnownPlayerPosition = playerTransform.position;
            Debug.Log("LKPP: " + lastKnownPlayerPosition);
            return Status.BH_SUCCESS;
        }
        else
        {
            SetLastSeenPosition();

            //Vi sätter inte "Target" om spelaren går bakom en vägg, och koden från Linecast dupliceras här
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
            //Om vi träffar något som inte är spelaren, så är siktlinjen bruten -> 
            bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(null);
            return false;
        }
        return true;
    }

    //Behöver inte ständigt uppdatera blackboardvärdet, reserverar det till LastKnownPosition blir relevant, 
    //alltså när spelaren lämnar vision, default value är Vector3.zero
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
