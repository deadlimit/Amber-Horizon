using UnityEngine;

public class VisualProximityCheck : BTNode
{
    //Knowledge of player
    private Transform playerTransform;
    private Vector3 lastKnownPlayerPosition;

    //Detection parameters
    private Vector3 heighOffset;
    private bool hasSeenPlayer;
    private float rangeRounding = 1f;
    private float visualRange;

    //Saving resources
    private int frameCounter;
    private int framesBetweenOverlapCasts = 33;
    private Status cachedValue = Status.BH_FAILURE;

   public VisualProximityCheck(BehaviourTree bt ) : base(bt)
    {
        visualRange = bt.owner.VisualRange;
        heighOffset = new Vector3(0, bt.owner.Collider.height, 0);
    }

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


        if (TargetTransformExists())
        {
            SetSpeedAndLastKnownPosition();
            return Status.BH_SUCCESS;
        }

        //Considered using if-else if here but that would only save one call to OverlapSphere,
        //and make the code a lot messier

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


            //"Target" is not assigned if line or angle of sight is broken, watch out for duplicate code here aswell
            if (!PlayerInLineOfSight() || !PlayerInAngleOfSight())
            {
                SetLastSeenPosition();
                ResetBlackboardValues();
                cachedValue = Status.BH_FAILURE;
                return Status.BH_FAILURE;
            }

            ResetPatrolPoint();
            bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(playerTransform);
            SetSpeedAndLastKnownPosition();

            return Status.BH_SUCCESS;
        }
        else
        {
          
            SetLastSeenPosition();
            ResetBlackboardValues();
            cachedValue = Status.BH_FAILURE;
            return Status.BH_FAILURE;
        }
    }
    private bool PlayerInLineOfSight()
    {
        if (Physics.Linecast(bt.ownerTransform.position + heighOffset, playerTransform.position + heighOffset, out var hitInfo, bt.owner.LineOfSightMask))
        {
            return false;
        }
        return true;
    }
    private void SetLastSeenPosition()
    {  
        //No need to constantly update blackboard value, its only done when the player leaves the AI's visual detection
        if (hasSeenPlayer)
        {
            bt.GetBlackBoardValue<Vector3>("LastSeenPosition").SetValue(lastKnownPlayerPosition);
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
    private void ResetBlackboardValues()
    {   
        //If visual detection has failed, reset the values of HasCalledForHelp, TargetTransform and AlerterTransform
        bt.GetBlackBoardValue<bool>("HasCalledForHelp").SetValue(false);
        bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(null);
        bt.GetBlackBoardValue<Transform>("AlerterTransform").SetValue(null);
    }

    private bool TargetTransformExists()
    {
        //If TargetTransform already exists we dont want to perform overlapSphere, this block also contains logic for if the AI was alerted to the player by
        //another AI, letting secondary AI use the Alerting AI:s position as origin for the Distance check
        if (bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue() != null)
        {
            //If AlerterTransform is set, this AI has been alerted by another and uses the Alerting AI:s position as origin for detection range
            Transform visualRangeOrigin = bt.GetBlackBoardValue<Transform>("AlerterTransform").GetValue() != null ?
                bt.GetBlackBoardValue<Transform>("AlerterTransform").GetValue() : bt.ownerTransform;

            //Target already exists and is in range of detection, then dont bother with OverlapSphere
            if (Vector3.Distance(visualRangeOrigin.position,
                bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue().position) < visualRange + rangeRounding)
            {
                return true;
            }
        }
        return false;
    }

    private void SetSpeedAndLastKnownPosition()
    {
        if (!playerTransform)
            playerTransform = bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue();

        lastKnownPlayerPosition = playerTransform.position;
        bt.ownerAgent.speed = bt.owner.MovementSpeedAttack;

        cachedValue = Status.BH_SUCCESS;
    }
}
