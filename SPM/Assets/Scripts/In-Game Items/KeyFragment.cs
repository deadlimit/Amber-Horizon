using UnityEngine;
using EventCallbacks;
public class KeyFragment : MonoBehaviour {
    
    [SerializeField] private KeyFragmentData keyFragmentData;
    
    private void Update() {
        transform.position = new Vector3(transform.position.x, transform.position.y - Mathf.Sin(keyFragmentData.BobSpeed * Time.time) * keyFragmentData.BobAmount, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) 
        {
            GateLock.KeysAcquired.Add(this);
            KeyPickUpEvent kpue = new KeyPickUpEvent();
            EventSystem<KeyPickUpEvent>.FireEvent(kpue);
            EventSystem<DisplayUIMessage>.FireEvent(new DisplayUIMessage("Key fragment acquired", 2, true));

            Destroy(gameObject);
        }
        
    }
}



