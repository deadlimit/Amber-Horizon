using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;


public class TransitUnit : InteractableObject {
    
    private static HashSet<TransitUnit> activatedTransitUnits = new HashSet<TransitUnit>();

    public Checkpoint AttachedCheckpoint { get; private set; }
    
    private Collider triggerCollider;
    
    private void OnEnable() {
        EventSystem<ExitTransitViewEvent>.RegisterListener(EnableTriggers);
    }

    private void OnDisable() {
        EventSystem<ExitTransitViewEvent>.UnregisterListener(EnableTriggers);
    }

    private void Awake() {
        triggerCollider = GetComponent<CapsuleCollider>();
        print(AttachedCheckpoint = transform.parent.GetComponentInChildren<Checkpoint>());
    }

    private void EnableTriggers(ExitTransitViewEvent viewEvent) {
        triggerCollider.enabled = true;
    }

    protected override void EnterTrigger(string UIMessage) {
        EventSystem<InteractTriggerEnterEvent>.FireEvent(new InteractTriggerEnterEvent(UIMessage));
        activatedTransitUnits.Add(this);
    }

    protected override void InsideTrigger(GameObject entity) {
        if (Input.GetKeyDown(KeyCode.F)) {
            EventSystem<EnterTransitViewEvent>.FireEvent(new EnterTransitViewEvent(activatedTransitUnits, this));
            triggerCollider.enabled = false;
        }
    }

    protected override void ExitTrigger() {
        EventSystem<InteractTriggerExitEvent>.FireEvent(new InteractTriggerExitEvent());
    }
}
