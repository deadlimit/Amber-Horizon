using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
public class KeyFragment : MonoBehaviour
{


    private void Start()
    {
        GateLock.keyList.Add(this);
    }

    private void OnDisable()
    {
        GateLock.keyList.Remove(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);

            //behöver inte ens skicka events om inte fler saker ska utföras!! 
            KeyPickUpEvent kpue = new KeyPickUpEvent();
            EventSystem<KeyPickUpEvent>.FireEvent(kpue);
        }
    }
}



