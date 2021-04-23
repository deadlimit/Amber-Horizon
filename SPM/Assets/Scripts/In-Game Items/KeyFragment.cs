using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks {
    public class KeyFragment : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                KeyPickUpEvent kpue = new KeyPickUpEvent();
                EventSystem<KeyPickUpEvent>.FireEvent(kpue);
                Destroy(gameObject);
            }
        }


    }
}
