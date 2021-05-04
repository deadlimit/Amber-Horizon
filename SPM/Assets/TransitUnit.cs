using System;
using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;

public class TransitUnit : MonoBehaviour {

    private static HashSet<TransitUnit> activatedTransitUnits = new HashSet<TransitUnit>();

    public Checkpoint AttachedCheckpoint { get; private set; }
    
    private Collider triggerCollider;
    
    private void OnEnable() {
        EventSystem<ExitTransitView>.RegisterListener(EnableTriggers);
    }

    private void OnDisable() {
        EventSystem<ExitTransitView>.UnregisterListener(EnableTriggers);
    }

    private void Awake() {
        triggerCollider = GetComponent<CapsuleCollider>();
        print(AttachedCheckpoint = transform.parent.GetComponentInChildren<Checkpoint>());
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            EventSystem<InteractTriggerEnter>.FireEvent(new InteractTriggerEnter("Press F to enter transit view"));
            activatedTransitUnits.Add(this);
        }
            
        
    }
    private void OnTriggerStay(Collider other) {
        if (Input.GetKeyDown(KeyCode.F)) {
            EventSystem<EnterTransitView>.FireEvent(new EnterTransitView(activatedTransitUnits, this));
            triggerCollider.enabled = false;
        }
            
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player"))
            EventSystem<InteractTriggerExit>.FireEvent(new InteractTriggerExit());
    }

    private void EnableTriggers(ExitTransitView view) {
        triggerCollider.enabled = true;
    }
    
}
