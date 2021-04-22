using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks {
    public class KeyFragment : MonoBehaviour
    {
        //lägg in sig i statisk lista vid OnEnable
        //när listan är tom/antalet uppfyllt -> unlock door event bla

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Destroy(gameObject);
                KeyPickUpEvent kpue = new KeyPickUpEvent();
                EventSystem<KeyPickUpEvent>.FireEvent(kpue);
            }
        }


    }
}
