using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class Gate : MonoBehaviour
{   
    Animator animator;

    private void OnEnable() 
    {
        animator = GetComponent<Animator>();
        EventSystem<UnlockEvent>.RegisterListener(DoorUnlocked);
    }
    private void OnDisable() => EventSystem<UnlockEvent>.UnregisterListener(DoorUnlocked);

    private void DoorUnlocked(UnlockEvent ue)
    {
        animator.SetTrigger("OpenGate");
    }


}
