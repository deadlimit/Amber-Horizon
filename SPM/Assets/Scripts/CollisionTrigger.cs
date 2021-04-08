using UnityEngine;

public class CollisionTrigger : MonoBehaviour {
    
    private BoxCollider collider;
    private LayerMask collisionMask;
    private RaycastHit hitInfo;
    
    private void Awake() => collisionMask = LayerMask.GetMask("Player");
    

    // Update is called once per frame
    void Update() {
        Collider[] coll = Physics.OverlapBox(collider.center, collider.size / 2, Quaternion.identity, collisionMask);
        
        if(coll.Length > 0)
            Debug.Log("Noce");
    }
}
