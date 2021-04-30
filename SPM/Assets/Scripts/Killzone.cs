using UnityEngine;
using EventCallbacks;

public class Killzone : MonoBehaviour {
        
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) 
        {
            Debug.Log("Collided with player");
            PlayerDiedEvent pde = new PlayerDiedEvent(other.gameObject);
            EventSystem<PlayerDiedEvent>.FireEvent(pde);
        }
            
    }
}
