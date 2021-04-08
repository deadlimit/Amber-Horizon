using UnityEngine;

public class BoxCaster : CollisionCaster {

    private BoxCollider attachedCollider;
    
    public BoxCaster(Collider collider, LayerMask mask) : base(mask) {
        attachedCollider = (BoxCollider)collider;
    }
    
    public override RaycastHit CastCollision(Vector3 origin, Vector3 direction, float distance) {
        Physics.BoxCast(origin, attachedCollider.size, direction.normalized, out var hit, Quaternion.identity, distance, CollisionMask);
        
        return hit;
    }

    public override Collider[] OverlapCast(Vector3 currentPosition) {
        return Physics.OverlapBox(currentPosition, attachedCollider.size, Quaternion.identity, CollisionMask);
    }
}
