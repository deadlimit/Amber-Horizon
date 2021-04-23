using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class UnlockListener : MonoBehaviour
    {
        private void OnEnable() => EventSystem<UnlockEvent>.RegisterListener(DoorUnlocked);
        private void OnDisable() => EventSystem<UnlockEvent>.UnregisterListener(DoorUnlocked);

        private void DoorUnlocked(UnlockEvent ue)
        {
            Debug.Log("Trigger aktiverad");
            gameObject.transform.parent.GetComponent<Door>().Unlock();
        }
    }
}
