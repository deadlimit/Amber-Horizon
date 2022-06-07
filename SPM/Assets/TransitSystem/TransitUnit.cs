using System.Collections.Generic;
using Cinemachine;
using EventCallbacks;
using UnityEngine;


public class TransitUnit : InteractableObject {

    private static HashSet<TransitUnit> activatedTransitUnits = new HashSet<TransitUnit>();
    
    [SerializeField] private Transform target;
    
    public Checkpoint AttachedCheckpoint { get; private set; }
    
    private Collider triggerCollider;
    
    private void OnEnable() {
        EventSystem<ResetCameraFocus>.RegisterListener(EnableTriggers);
        EventSystem<CheckPointActivatedEvent>.RegisterListener(ActivateTransitUnit);
        EventSystem<NewLevelLoadedEvent>.RegisterListener(ClearTransitUnits);
        
    }

    private void OnDisable() {
        EventSystem<ResetCameraFocus>.UnregisterListener(EnableTriggers);
        EventSystem<CheckPointActivatedEvent>.UnregisterListener(ActivateTransitUnit);
        EventSystem<NewLevelLoadedEvent>.UnregisterListener(ClearTransitUnits);
    }

    private void Awake() {
        triggerCollider = GetComponent<CapsuleCollider>();
        AttachedCheckpoint = transform.parent.GetComponentInChildren<Checkpoint>();
    }

    private void EnableTriggers(ResetCameraFocus viewEvent) {
        triggerCollider.enabled = true;
    }

    protected override void EnterTrigger(string UIMessage) {
        EventSystem<InteractTriggerEnterEvent>.FireEvent(new InteractTriggerEnterEvent(UIMessage));
    }
    
    protected override void InsideTrigger(GameObject entity) {
        
        if (Input.GetKeyDown(KeyCode.E)) {

            TransitCameraFocusInfo info = new TransitCameraFocusInfo {TransitUnits = activatedTransitUnits, ActivatedTransitUnit = this};

            EventSystem<EnterTransitViewEvent>.FireEvent(new EnterTransitViewEvent(info));
            //triggerCollider.enabled = false;
        }

        //for closing the menu
        if (Input.GetKeyDown(KeyCode.F))
        {
            EventSystem<ResetCameraFocus>.FireEvent(null);
        }
    }
    
    protected override void ExitTrigger() {
        EventSystem<InteractTriggerExitEvent>.FireEvent(new InteractTriggerExitEvent());
    }

    private void ActivateTransitUnit(CheckPointActivatedEvent checkPointActivatedEvent) {
        if(checkPointActivatedEvent.checkpoint.GetInstanceID().Equals(AttachedCheckpoint.GetInstanceID()))
            activatedTransitUnits.Add(this);
    }

    private void ClearTransitUnits(NewLevelLoadedEvent transitEvent) {
        activatedTransitUnits.Clear();
    }
}
