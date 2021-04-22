using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks {
    public class KeyFragment : MonoBehaviour
    {
        static List<KeyFragment> keyList;
        private void OnEnable()
        {
            keyList.Add(this);
        }

        private void OnDisable()
        {
            //tar vi bort this från listan? Räknar vi ned? 
        }

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
