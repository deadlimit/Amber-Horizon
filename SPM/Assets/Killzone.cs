using UnityEngine;

public class Killzone : MonoBehaviour {
    
    //TODO Gör om killzone-kollision så den triggar ett event istället. 

    private CheckpointManager checkpointManager;

    private void Start() {
        checkpointManager = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();
        if (checkpointManager == null)
            Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            checkpointManager.ResetPlayerPosition();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PhysicsComponent>().velocity = Vector3.zero;
        }
            
    }
}
