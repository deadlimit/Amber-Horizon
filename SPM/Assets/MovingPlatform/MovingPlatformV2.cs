using UnityEngine;

public class MovingPlatformV2 : MonoBehaviour, IBlackHoleBehaviour {

    private PhysicsComponent physics;
    public float multiplier = 1f;
    private Vector3 maxBack;
    private Vector3 maxFront;
    private Vector3 startPos;
    private Transform thisTransform;
    private Vector3 movementDirection;

    public BlackHole activeBlackHole;
    private float dotProduct;

    private float frontDistance;
    private float backDistance;


    [SerializeField] private float MovementSpeed;
    [SerializeField] private float MaxMovementLengthBack;
    [SerializeField] private float MaxMovementLengthFront;


    private void Awake() {
        physics = GetComponent<PhysicsComponent>();
        thisTransform = GetComponent<Transform>();
        maxBack = transform.position + -transform.forward * MaxMovementLengthBack;
        maxFront = transform.position + transform.forward * MaxMovementLengthFront;
        startPos = transform.position;

        frontDistance = Vector3.Distance(startPos, maxFront);
        backDistance = Vector3.Distance(startPos, maxBack);
        
    }

    public void Update() {
        Debug.DrawLine(transform.position,  maxFront, Color.red);
        Debug.DrawLine(transform.position,  maxBack, Color.green);
       
        ClampPosition();
        physics.velocity = (movementDirection * multiplier);
        movementDirection = Vector3.zero;

        CheckBelowStoppingSpeed();
    }
    
    public void BlackHoleBehaviour(BlackHole blackhole) 
    {
        activeBlackHole = blackhole;
        dotProduct = Vector3.Dot(transform.forward, (activeBlackHole.transform.position - transform.position).normalized);
        Debug.Log("Dotproduct: " + dotProduct);
    }

    private void CheckBelowStoppingSpeed() {
        if (physics.velocity.magnitude < 0.1)
            physics.velocity = Vector3.zero;
    }

    public Vector3 GetVelocity() { return physics.velocity; }
    private void ClampPosition()
    {
        if (dotProduct > 0.1f)
        {
            if (AllowedToMoveForward())
                movementDirection = transform.forward * (MovementSpeed * Time.fixedDeltaTime);
            else
                physics.StopVelocity();
        }
        else if (dotProduct < -0.1f)
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
}
