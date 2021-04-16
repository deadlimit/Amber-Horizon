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
            //door.Trigger.SetActive typ
            //triggern är sedan det som styr animation och collider, så att dörren kan öppnas när spelaren närmar sig
            
            //Vet att man kan lägga scriptet på dörren direkt men det kändes tydligare att ha lyssnaren ett eget GO 
            gameObject.transform.parent.GetComponent<Door>().Unlock();
            Debug.Log("Trigger aktiverad");
        }
    }
}
