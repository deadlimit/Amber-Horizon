using System;
using EventCallbacks;
using UnityEngine;

public class TransitUnit : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player"))
            EventSystem<InteractTriggerEnter>.FireEvent(new InteractTriggerEnter("Press F to transit"));
        

    }

    private void OnTriggerStay(Collider other) {
        if (Input.GetKeyDown(KeyCode.F))
            EventSystem<EnterTransitView>.FireEvent(null);
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player"))
            EventSystem<InteractTriggerExit>.FireEvent(new InteractTriggerExit());
    }
}
