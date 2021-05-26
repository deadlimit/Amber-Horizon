using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
public class GateLock : InteractableObject
{
    public static readonly List<KeyFragment> KeyList = new List<KeyFragment>();
    public static readonly List<KeyFragment> KeysAcquired = new List<KeyFragment>();
    
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
            KeyList.Clear();
            KeysAcquired.Clear();
            UnlockGateSequence();
        }

    }
    
    private void KeyPickUp(KeyPickUpEvent kpue)
    {
        if (KeyList.Count == KeysAcquired.Count)
        {
            interaction.enabled = true;
        }
    }

    protected override void EnterTrigger(string UIMessage) {
        UIMessage = KeyList.Count == KeysAcquired.Count ? UIMessage : "Missing key fragments: " + (KeyList.Count - KeysAcquired.Count);
        EventSystem<InteractTriggerEnterEvent>.FireEvent(new InteractTriggerEnterEvent(UIMessage));
    }

    protected override void InsideTrigger(GameObject player) {
        
        if (KeysAcquired.Count == KeyList.Count && Input.GetKeyDown(KeyCode.F)) {
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

