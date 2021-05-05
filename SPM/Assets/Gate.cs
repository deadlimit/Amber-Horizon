using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class Gate : MonoBehaviour
{   
    Animator animator;

    public GameObject dustVFX_Right;
    public GameObject dustVFX_Left;

    private void OnEnable() 
    {
        animator = GetComponent<Animator>();
        EventSystem<UnlockEvent>.RegisterListener(DoorUnlocked);
    }
    private void OnDisable() => EventSystem<UnlockEvent>.UnregisterListener(DoorUnlocked);

    private void DoorUnlocked(UnlockEvent ue)
    {
        animator.SetTrigger("OpenGate");
        dustVFX_Right.SetActive(true);
        dustVFX_Left.SetActive(true);
    }


}
