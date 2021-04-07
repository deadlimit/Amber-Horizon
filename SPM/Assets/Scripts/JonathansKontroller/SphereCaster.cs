using System;
using UnityEngine;

public class SphereCaster : CollisionCaster {
    
    private SphereCollider attachedCollider;
    
    public SphereCaster(Collider collider, LayerMask mask) : base(mask) {
        attachedCollider = (SphereCollider)collider;
    }
    
    public override RaycastHit CastCollision(Vector3 origin, Vector3 direction, float distance) {
        throw new NotImplementedException();
    }

    public override Collider[] OverlapCast(Vector3 currentPosition) {
        throw new NotImplementedException();
    }
}
