using UnityEngine;

public class WallPiece : MonoBehaviour, IBlackHoleBehaviour  {

    private bool insideBlackHole;
    private BlackHole blackhole;
    public LayerMask collisionMask;

    private MeshCollider activeCollider;

    private void Awake() {
        activeCollider = GetComponent<MeshCollider>();
    }
    
    public void BlackHoleBehaviour(BlackHole blackHole) {
        insideBlackHole = true;
        blackhole = blackHole;
        PhysicsComponent physics = gameObject.AddComponent<PhysicsComponent>();
        physics.collisionMask = collisionMask;
        physics.gravity = 0;
        activeCollider.enabled = false;
        print("wall piece");
    }

    private void Update() {
        if (!insideBlackHole || blackhole == null) return;
        
        transform.position = Vector3.Lerp(transform.position, blackhole.transform.position, Time.deltaTime * 10);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 10);
        
        Destroy(gameObject, 2);
    }
}
