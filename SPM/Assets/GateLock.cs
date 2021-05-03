using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class GateLock : MonoBehaviour
    {
        public static List<KeyFragment> keyList = new List<KeyFragment>();
        public static List<KeyFragment> keysAcquired = new List<KeyFragment>();
        private BoxCollider interaction;

        private void OnEnable()
        {
            EventSystem<KeyPickUpEvent>.RegisterListener(KeyPickUp);
            interaction = GetComponent<BoxCollider>();
        }
        private void OnDisable() => EventSystem<KeyPickUpEvent>.UnregisterListener(KeyPickUp);

        private void KeyPickUp(KeyPickUpEvent kpue)
        {
            if (keyList.Count == keysAcquired.Count)
            {
                interaction.enabled = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (keyList.Count == keysAcquired.Count && other.gameObject.tag == "Player")
            {
                UnlockEvent ue = new UnlockEvent();
                EventSystem<UnlockEvent>.FireEvent(ue);
            }
        }


    }
}
