using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
public class KeyFragment : MonoBehaviour
{
    private void OnEnable()
    {
        GateLock.keyList.Add(this);
    }

    private void OnDisable()
    {
        GateLock.keysAcquired.Add(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            //behöver inte ens skicka events om inte fler saker ska utföras
            KeyPickUpEvent kpue = new KeyPickUpEvent();
            EventSystem<KeyPickUpEvent>.FireEvent(kpue);
            EventSystem<DisplayUIMessage>.FireEvent(new DisplayUIMessage("Key fragment aquired", 2, true));

            Destroy(gameObject);
        }
    }
}



