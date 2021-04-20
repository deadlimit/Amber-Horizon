using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class KeyListener : MonoBehaviour
    {
        private void OnEnable() => EventSystem<KeyPickUpEvent>.RegisterListener(KeyPickUp);
        private void OnDisable() => EventSystem<KeyPickUpEvent>.UnregisterListener(KeyPickUp);

        private void KeyPickUp (KeyPickUpEvent kpue)
        {
            Debug.Log("nyckelfragment upplockat");
            if (GameManager.gameManager.gameVariables.RequiredNoOfKeys())
            {
                UnlockEvent ue = new UnlockEvent();
                EventSystem<UnlockEvent>.FireEvent(ue);
            }
            //Lagra antalet nycklar.. någonstans. Game manager? 
        }

    }
}
