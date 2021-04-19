using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DestructorFist : MonoBehaviour {

    private SphereCollider coll;
    
    public float hitForce;
    
    private void Awake() {
        coll = GetComponent<SphereCollider>();
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {

            print("worked");
            PhysicsComponent player = other.GetComponent<PhysicsComponent>();
            
            player.GetComponent<PhysicsComponent>().AddForce(-player.transform.forward * hitForce);
            coll.enabled = false;
            enabled = false;
        }
    }
}
