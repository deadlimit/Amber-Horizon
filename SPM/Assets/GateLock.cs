using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
public class GateLock : InteractableObject
{
    public static List<KeyFragment> keyList = new List<KeyFragment>();
    public static List<KeyFragment> keysAcquired = new List<KeyFragment>();
    private BoxCollider interaction;

    [Header("Fuskknapp, sätt till true för att gaten ska öppna direkt (debug)")]
    [SerializeField] private bool OpenDoorWithoutKeys;
    
    private void OnEnable() {
        EventSystem<KeyPickUpEvent>.RegisterListener(KeyPickUp);
        interaction = GetComponent<BoxCollider>();
    }
    private void OnDisable() => EventSystem<KeyPickUpEvent>.UnregisterListener(KeyPickUp);

    private void Start() {
        if (OpenDoorWithoutKeys) {
            keyList.Clear();
            keysAcquired.Clear();
            UnlockGateSequence();
        }

    }
    
    private void KeyPickUp(KeyPickUpEvent kpue)
    {
        if (keyList.Count == keysAcquired.Count)
        {
            interaction.enabled = true;
        }
    }

    protected override void EnterTrigger(string UIMessage) {
        UIMessage = keyList.Count == keysAcquired.Count ? UIMessage : "Missing key fragments: " + (keyList.Count - keysAcquired.Count);
        EventSystem<InteractTriggerEnterEvent>.FireEvent(new InteractTriggerEnterEvent(UIMessage));
    }

    protected override void InsideTrigger(GameObject player) {
        
        if (keysAcquired.Count == keyList.Count && Input.GetKeyDown(KeyCode.F)) {
            UnlockGateSequence();
        }
    }

    protected override void ExitTrigger() {
        EventSystem<InteractTriggerExitEvent>.FireEvent(new InteractTriggerExitEvent());
    }

    private void UnlockGateSequence() {
        UnlockEvent ue = new UnlockEvent();
        EventSystem<UnlockEvent>.FireEvent(ue);
        EventSystem<InteractTriggerExitEvent>.FireEvent(new InteractTriggerExitEvent());
        interaction.enabled = false;
        Destroy(this);
    }
    

}

