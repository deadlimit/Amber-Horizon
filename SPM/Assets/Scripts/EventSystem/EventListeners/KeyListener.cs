using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EventCallbacks
{
    public class KeyListener : MonoBehaviour
    {
        private static List<KeyFragment> keyList = new List<KeyFragment>();
        private static List<KeyFragment> keysAcquired = new List<KeyFragment>();
        private BoxCollider interaction;
        public Text keyText; 
        
        private void OnEnable()
        {
            EventSystem<KeyPickUpEvent>.RegisterListener(KeyPickUp);
            interaction = GetComponent<BoxCollider>();
        }
        private void OnDisable() => EventSystem<KeyPickUpEvent>.UnregisterListener(KeyPickUp);

        private void KeyPickUp(KeyPickUpEvent kpue)
        {
            if (keyList.Count <= 0)
            {
                interaction.enabled = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(keyList.Count <= 0 && other.gameObject.CompareTag("Player"))
            {
                UnlockEvent ue = new UnlockEvent();
                EventSystem<UnlockEvent>.FireEvent(ue);
            }
        }
        
    }
}
