using UnityEngine;

public class MovingPlatformV2 : MonoBehaviour, IBlackHoleBehaviour {

    private PhysicsComponent physics;

    private Vector3 maxBack;
    private Vector3 maxFront;
    
    public float MovementSpeed;
    public float MaxMovementLength;
    
    private void Awake() {
        physics = GetComponent<PhysicsComponent>();
        maxBack = transform.position + -transform.forward * MaxMovementLength;
        maxFront = transform.position + transform.forward * MaxMovementLength;
        
    }

    public void Update() {
        Debug.DrawLine(transform.position,  maxFront, Color.red);
        Debug.DrawLine(transform.position,  maxBack, Color.green);
        physics.velocity = Vector3.zero;
    }
    
    public void BlackHoleBehaviour(BlackHole blackhole) {

       /* if (transform.position.x > maxFront.x && transform.position.z > maxFront.z) 
            transform.position = maxFront;
        
        if (transform.position.x < maxBack.x && transform.position.z < maxBack.z) 
            transform.position = maxBack;
        */
        Vector3 blackHoleVector3 = (blackhole.transform.position - transform.position).normalized;

        float dotProduct = Vector3.Dot(transform.forward, blackHoleVector3);

        Vector3 movementDirection = Vector3.zero;
        
        
        if(dotProduct > 0.1f)
            movementDirection = transform.forward * (MovementSpeed * Time.deltaTime);
        else if(dotProduct < -0.1f)
            movementDirection = -transform.forward * (MovementSpeed * Time.deltaTime);
        else {
            movementDirection = Vector3.zero;
        }
        
        physics.velocity = Vector3.zero;
        physics.AddForce(movementDirection);
    }
}
