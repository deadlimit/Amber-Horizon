using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class MovingPlatformV2 : MonoBehaviour, IBlackHoleBehaviour {

    private PhysicsComponent physics;
    public float multiplier = 1f;
    [SerializeField]private Vector3 maxBack;
    [SerializeField]private Vector3 maxFront;
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

        /*if(!physics.AffectedByBlackHoleGravity)
            physics.velocity = Vector3.zero;*/
        physics.ApplyAirResistance();
        PreventOutOfBounds();
    }
    
    public void BlackHoleBehaviour(BlackHole blackhole) {
        
        //PreventOutOfBounds();
        
        Vector3 blackHoleVector3 = (blackhole.transform.position - transform.position).normalized;

        float dotProduct = Vector3.Dot(transform.forward, blackHoleVector3);

        Vector3 movementDirection = Vector3.zero;

        if (dotProduct > 0.1f)
        {
            if (Vector3.Dot(transform.forward, transform.position - startPos) < 0 || Vector3.Distance(transform.position, startPos) < frontDistance)
                movementDirection = transform.forward * (MovementSpeed * Time.deltaTime);
        }
        else if (dotProduct < -0.1f)
        {
            if (Vector3.Dot(transform.forward, transform.position - startPos) > 0 || Vector3.Distance(transform.position, startPos) < backDistance)
                movementDirection = -transform.forward * (MovementSpeed * Time.deltaTime);
        }


        physics.velocity = movementDirection * multiplier;
    }

    private void PreventOutOfBounds() {
        if (physics.velocity.magnitude < 0.1)
            physics.velocity = Vector3.zero;
    }

    public Vector3 GetVelocity() { return physics.velocity; }
}
