using UnityEngine;
using EventCallbacks;
public class KeyFragment : MonoBehaviour {
    
    [SerializeField] private KeyFragmentData keyFragmentData;
    
    private void OnEnable()
    {
        GateLock.KeyList.Add(this);
    }
    
    private void Update() {
        transform.position = new Vector3(transform.position.x, transform.position.y - Mathf.Sin(keyFragmentData.BobSpeed * Time.time) * keyFragmentData.BobAmount, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GateLock.KeysAcquired.Add(this);
            KeyPickUpEvent kpue = new KeyPickUpEvent();
            EventSystem<KeyPickUpEvent>.FireEvent(kpue);
            EventSystem<DisplayUIMessage>.FireEvent(new DisplayUIMessage("Key fragment aquired", 2, true));
            
            Destroy(gameObject);
        }
    }
}



