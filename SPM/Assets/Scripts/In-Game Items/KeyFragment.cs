using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks {
    public class KeyFragment : MonoBehaviour
    {
        //l�gg in sig i statisk lista vid OnEnable
        //n�r listan �r tom/antalet uppfyllt -> unlock door event bla

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
