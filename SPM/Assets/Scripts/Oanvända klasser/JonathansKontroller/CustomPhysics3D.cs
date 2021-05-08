using UnityEngine;


public class CustomPhysics3D : CustomPhysics {

    private Collider attachedCollider;

    private CollisionCaster collisionCaster;
    private void OnEnable() {
        attachedCollider = GetComponent<Collider>();
        
        if (attachedCollider is BoxCollider)
            collisionCaster = new BoxCaster(attachedCollider, CollisionMask);
        
        if(attachedCollider is SphereCollider)
            collisionCaster = new SphereCaster(attachedCollider, CollisionMask);
        
        if(attachedCollider is CapsuleCollider)
            collisionCaster = new CapsuleCaster(attachedCollider, CollisionMask);
    }

    private void Update() {
        
        ApplyGravity();
        
        CheckForCollisions();
        
        ApplyMovement();
        
        MoveOutOfGeometry();
        
    }
    
    private void CheckForCollisions() {
        
        RaycastHit hitInfo = collisionCaster.CastCollision(transform.position, velocity.normalized, velocity.magnitude * Time.deltaTime + SkinWidth);
        
        if (!hitInfo.collider) return;
        
        RaycastHit newHitInfo = collisionCaster.CastCollision(transform.position, -hitInfo.normal, hitInfo.distance);
        
        velocity += -newHitInfo.normal * (newHitInfo.distance - SkinWidth);
        
        Vector3 normalForce = CalculateNormalForce(ref velocity, hitInfo.normal);
        velocity += normalForce;
        
        ApplyFriction(normalForce.magnitude, hitInfo.collider);
        CheckForCollisions();
    }
    


    private void MoveOutOfGeometry() {
        Collider[] colliders = collisionCaster.OverlapCast(transform.position);
        
        if (colliders.Length < 1) return;
        
        foreach (Collider currentCollider in colliders) {
            Physics.ComputePenetration( attachedCollider,
                                        transform.position, 
                                        transform.rotation, 
                                        currentCollider,
                                        currentCollider.transform.position, 
                                        currentCollider.transform.rotation, 
                                        out Vector3 seperationVector,
                                               out float distance);

            Vector3 seperationVec = seperationVector * distance;
            transform.position += seperationVec + seperationVec.normalized * SkinWidth;
            velocity += CalculateNormalForce(ref velocity, seperationVector);
        }
        
    }

    public RaycastHit CollisionCast(Vector3 direction, float distance) {
        return collisionCaster.CastCollision(transform.position, direction, distance);
    }
    
}
