using System;
using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;


public class TransitUnit : InteractableObject {

    private static HashSet<TransitUnit> activatedTransitUnits = new HashSet<TransitUnit>();

    [SerializeField] private Vector3 newOffset;
    [SerializeField] private Transform target;
    public Checkpoint AttachedCheckpoint { get; private set; }
    
    private Collider triggerCollider;
    
    private void OnEnable() {
        EventSystem<ExitTransitViewEvent>.RegisterListener(EnableTriggers);
        EventSystem<CheckPointActivatedEvent>.RegisterListener(ActivateTransitUnit);
        EventSystem<StartSceneTransitEvent>.RegisterListener(ClearTransitUnits);
    }

    private void OnDisable() {
        EventSystem<ExitTransitViewEvent>.UnregisterListener(EnableTriggers);
        EventSystem<CheckPointActivatedEvent>.UnregisterListener(ActivateTransitUnit);
        EventSystem<StartSceneTransitEvent>.RegisterListener(ClearTransitUnits);
    }

    private void Awake() {
        triggerCollider = GetComponent<CapsuleCollider>();
        AttachedCheckpoint = transform.parent.GetComponentInChildren<Checkpoint>();
    }

    private void EnableTriggers(ExitTransitViewEvent viewEvent) {
        triggerCollider.enabled = true;
    }

    protected override void EnterTrigger(string UIMessage) {
        EventSystem<InteractTriggerEnterEvent>.FireEvent(new InteractTriggerEnterEvent(UIMessage));
    }
    
    protected override void InsideTrigger(GameObject entity) {
        
        if (Input.GetKeyDown(KeyCode.F)) {

            TransitCameraFocusInfo info = new TransitCameraFocusInfo();
            info.TransitUnits = activatedTransitUnits;
            info.ActivatedTransitUnit = this;
            
            EventSystem<NewCameraFocus>.FireEvent(new NewCameraFocus(target.transform));
            EventSystem<EnterTransitViewEvent>.FireEvent(new EnterTransitViewEvent(info));
            triggerCollider.enabled = false;
        }
    }
    
    protected override void ExitTrigger() {
        EventSystem<InteractTriggerExitEvent>.FireEvent(new InteractTriggerExitEvent());
    }

    private void ActivateTransitUnit(CheckPointActivatedEvent checkPointActivatedEvent) {
        if(checkPointActivatedEvent.checkpoint.GetInstanceID().Equals(AttachedCheckpoint.GetInstanceID()))
            activatedTransitUnits.Add(this);
    }

    private void ClearTransitUnits(StartSceneTransitEvent transitEvent) {
        activatedTransitUnits.Clear();
    }
}
