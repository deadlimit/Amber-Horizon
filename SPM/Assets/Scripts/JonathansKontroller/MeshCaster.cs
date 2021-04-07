using UnityEngine;


//Hur fan kollar man kollision med en mesh collider?
public class MeshCaster : CollisionCaster {

    private MeshCollider attachedCollider;

    private Rigidbody rigidbody;
    
    public MeshCaster(Collider collider, LayerMask mask) : base(mask) {
        attachedCollider = (MeshCollider) collider;
        rigidbody = collider.gameObject.AddComponent<Rigidbody>();

        rigidbody.isKinematic = true;

    }
    
    
    public override RaycastHit CastCollision(Vector3 origin, Vector3 direction, float distance) {
        rigidbody.SweepTest(direction, out var hit, distance);

        return hit;
    }

    public override Collider[] OverlapCast(Vector3 currentPosition) {
        return Physics.OverlapBox(currentPosition, Vector3.up, Quaternion.identity, CollisionMask);
    }


}
