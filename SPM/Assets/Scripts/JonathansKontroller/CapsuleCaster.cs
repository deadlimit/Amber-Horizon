using UnityEngine;

public class CapsuleCaster : CollisionCaster {

    private CapsuleCollider attachedCollider;

    private Vector3 colliderTopHalf;
    private Vector3 colliderBottomHalf;
    
    public CapsuleCaster(Collider collider, LayerMask mask) : base(mask) {
        attachedCollider = (CapsuleCollider)collider;
    }
    
    public override RaycastHit CastCollision(Vector3 origin, Vector3 direction, float distance) {
        UpdateColliderPosition(origin);
        
        Physics.CapsuleCast(colliderTopHalf, colliderBottomHalf, attachedCollider.radius, direction.normalized, out var hitInfo, distance,  CollisionMask);
        
        return hitInfo;
    }

    public override Collider[] OverlapCast(Vector3 currentPosition) {
        UpdateColliderPosition(currentPosition);
        return Physics.OverlapCapsule(colliderTopHalf, colliderBottomHalf, attachedCollider.radius, CollisionMask);
    }

    private void UpdateColliderPosition(Vector3 currentPosition) {
        colliderTopHalf = (currentPosition + attachedCollider.center) + Vector3.up * (attachedCollider.height / 2 - attachedCollider.radius);
        colliderBottomHalf = (currentPosition + attachedCollider.center) + Vector3.down * (attachedCollider.height / 2 - attachedCollider.radius);
    }
}
