using UnityEngine;

public class MovingPlatformV2 : MonoBehaviour, IBlackHoleBehaviour {

    private PhysicsComponent physics;
    public float multiplier = 1f;
    private Vector3 maxBack;
    private Vector3 maxFront;
    private Vector3 startPos;

    private float frontDistance;
    private float backDistance;

    public float MovementSpeed;
    public float MaxMovementLengthBack;
    public float MaxMovementLengthFront;
    
    private void Awake() {
        physics = GetComponent<PhysicsComponent>();
        maxBack = transform.position + -transform.forward * MaxMovementLengthBack;
        maxFront = transform.position + transform.forward * MaxMovementLengthFront;
        startPos = transform.position;

        frontDistance = Vector3.Distance(startPos, maxFront);
        backDistance = Vector3.Distance(startPos, maxBack);
    }

    public void Update() {
        Debug.DrawLine(transform.position,  maxFront, Color.red);
        Debug.DrawLine(transform.position,  maxBack, Color.green);
        
        PreventOutOfBounds();
    }
    
    public void BlackHoleBehaviour(BlackHole blackhole) {
            
        Vector3 blackHoleVector3 = (blackhole.transform.position - transform.position).normalized;

        float dotProduct = Vector3.Dot(transform.forward, blackHoleVector3);

        Vector3 movementDirection = Vector3.zero;

        if (dotProduct > 0.1f)
        {
            if (Vector3.Dot(transform.forward, transform.position - startPos) < 0 || Vector3.Distance(transform.position, startPos) < frontDistance)
                movementDirection = transform.forward * (MovementSpeed * Time.fixedDeltaTime);
        }
        else if (dotProduct < -0.1f)
        {
            if (Vector3.Dot(transform.forward, transform.position - startPos) > 0 || Vector3.Distance(transform.position, startPos) < backDistance)
                movementDirection = -transform.forward * (MovementSpeed * Time.fixedDeltaTime);
        }


        physics.velocity  = (movementDirection * multiplier);
    }

    private void PreventOutOfBounds() {
        
        if(Vector3.Distance(transform.position, maxFront) < .1f || Vector3.Distance(transform.position, maxBack) < .1f)
            physics.velocity = Vector3.zero;
        
        if (physics.velocity.magnitude < 0.1f)
            physics.velocity = Vector3.zero;
    }

    public Vector3 GetVelocity() { return physics.velocity; }
}
