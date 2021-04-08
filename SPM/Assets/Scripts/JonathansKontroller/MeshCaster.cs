using UnityEngine;


//Hur fan kollar man kollision med en mesh collider?
public class MeshCaster : CollisionCaster {

    private MeshCollider attachedCollider;

    
    public MeshCaster(Collider collider, LayerMask mask) : base(mask) {
        attachedCollider = (MeshCollider) collider;
    }
    
    
    public override RaycastHit CastCollision(Vector3 origin, Vector3 direction, float distance) {
        Physics.Raycast(attachedCollider.transform.position, Vector3.zero, out var hit, 0, LayerMask.GetMask("UI"));
        return hit;
    }

    public override Collider[] OverlapCast(Vector3 currentPosition) {
        return new Collider[0];
    }


}
