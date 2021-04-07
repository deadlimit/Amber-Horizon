using UnityEngine;

public abstract class CollisionCaster {

    protected LayerMask CollisionMask;

    protected CollisionCaster(LayerMask mask) => CollisionMask = mask;
    
    public abstract RaycastHit CastCollision(Vector3 origin, Vector3 direction, float distance);

    public abstract Collider[] OverlapCast(Vector3 currentPosition);
}
