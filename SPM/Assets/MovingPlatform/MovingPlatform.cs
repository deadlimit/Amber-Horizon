using UnityEngine;
using EventCallbacks;

public class MovingPlatform : MonoBehaviour, IBlackHoleBehaviour {


    [SerializeField] private float MovementSpeed;
    [SerializeField] private float MaxMovementLengthBack;
    [SerializeField] private float MaxMovementLengthFront;
    [SerializeField] private float multiplier = 1f;

    private float dotProductThreshold = 0.1f;
    private float speedThreshold = 0.1f;
    private BlackHole activeBlackHole;
    private PhysicsComponent physics;
   
    //Clamping positions
    private float frontDistance;
    private float backDistance;
    private float dotProduct;
    private Vector3 maxBack;
    private Vector3 maxFront;
    private Vector3 startPos;
    private Vector3 movementDirection;
    private Vector3 resetPos;


    private void Awake() 
    {
        physics = GetComponent<PhysicsComponent>();
        maxBack = transform.position + -transform.forward * MaxMovementLengthBack;
        maxFront = transform.position + transform.forward * MaxMovementLengthFront;
        startPos = transform.position;
        resetPos = transform.localPosition;

        Debug.Log(transform.localPosition);

        frontDistance = Vector3.Distance(startPos, maxFront);
        backDistance = Vector3.Distance(startPos, maxBack);

        EventSystem<PlayerReviveEvent>.RegisterListener(ResetPlatformPosition);
    }

    public void Update() 
    {       
        ClampPosition();
        physics.velocity = (movementDirection * multiplier);
        movementDirection = Vector3.zero;

        CheckBelowSpeedThreshold();
    }
    
    public void BlackHoleBehaviour(BlackHole blackhole) 
    {
        activeBlackHole = blackhole;
        dotProduct = Vector3.Dot(transform.forward, (activeBlackHole.transform.position - transform.position).normalized);
    }

    private void CheckBelowSpeedThreshold() 
    {
        if (physics.velocity.magnitude < speedThreshold)
            physics.velocity = Vector3.zero;
    }

    public Vector3 GetVelocity() { return physics.velocity; }
    private void ClampPosition()
    {
        if (dotProduct > dotProductThreshold)
        {
            if (AllowedToMoveForward())
                movementDirection = transform.forward * (MovementSpeed * Time.fixedDeltaTime);
            else
                physics.StopVelocity();
        }
        else if (dotProduct < -dotProductThreshold)
        {
            if (AllowedToMoveBackward())
                movementDirection = -transform.forward * (MovementSpeed * Time.fixedDeltaTime);
            else
                physics.StopVelocity();
        }
    }
    private bool AllowedToMoveForward()
    {
        return (Vector3.Dot(transform.forward, transform.position - startPos) < 0 || Vector3.Distance(transform.position, startPos) < frontDistance);
    }
    private bool AllowedToMoveBackward()
    {
        return (Vector3.Dot(transform.forward, transform.position - startPos) > 0 || Vector3.Distance(transform.position, startPos) < backDistance);
    } 
    private void DebugMaxDistance()
    {
        Debug.DrawLine(transform.position, maxFront, Color.red);
        Debug.DrawLine(transform.position, maxBack, Color.green);
    }

    private void ResetPlatformPosition(PlayerReviveEvent playerReviveEvent) 
    {
        Debug.Log("In MovingPlatform. resetPos is " + resetPos);

        physics.velocity = (transform.forward * (MovementSpeed * Time.fixedDeltaTime) * multiplier);
        movementDirection = Vector3.zero;

        transform.localPosition = resetPos;

        Debug.Log("In MovingPlatform. localPos is " + transform.localPosition);

        //physics.StopVelocity();
        //movementDirection = Vector3.zero;
    }
}
