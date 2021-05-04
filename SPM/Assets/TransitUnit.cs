using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;


public class TransitUnit : InteractableObject {
    
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

    private void EnableTriggers(ExitTransitView view) {
        triggerCollider.enabled = true;
    }

    protected override void EnterTrigger(string UIMessage) {
        EventSystem<InteractTriggerEnter>.FireEvent(new InteractTriggerEnter(UIMessage));
        activatedTransitUnits.Add(this);
    }

    protected override void InsideTrigger() {
        if (Input.GetKeyDown(KeyCode.F)) {
            EventSystem<EnterTransitView>.FireEvent(new EnterTransitView(activatedTransitUnits, this));
            triggerCollider.enabled = false;
        }
    }

    protected override void ExitTrigger() {
        EventSystem<InteractTriggerExit>.FireEvent(new InteractTriggerExit());
    }
}
