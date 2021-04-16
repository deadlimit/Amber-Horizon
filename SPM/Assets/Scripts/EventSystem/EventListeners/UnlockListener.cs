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
            //triggern �r sedan det som styr animation och collider, s� att d�rren kan �ppnas n�r spelaren n�rmar sig
            
            //Vet att man kan l�gga scriptet p� d�rren direkt men det k�ndes tydligare att ha lyssnaren ett eget GO 
            gameObject.transform.parent.GetComponent<Door>().Unlock();
            Debug.Log("Trigger aktiverad");
        }
    }
}
