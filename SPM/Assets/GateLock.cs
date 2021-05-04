using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
public class GateLock : InteractableObject
{
    public static List<KeyFragment> keyList = new List<KeyFragment>();
    public static List<KeyFragment> keysAcquired = new List<KeyFragment>();
    private BoxCollider interaction;

    private void OnEnable()
    {
        EventSystem<KeyPickUpEvent>.RegisterListener(KeyPickUp);
        interaction = GetComponent<BoxCollider>();
    }
    private void OnDisable() => EventSystem<KeyPickUpEvent>.UnregisterListener(KeyPickUp);

    private void KeyPickUp(KeyPickUpEvent kpue)
    {
        if (keyList.Count == keysAcquired.Count)
        {
            interaction.enabled = true;
        }
    }

    protected override void EnterTrigger(string UIMessage) {
        UIMessage = keyList.Count == keysAcquired.Count ? UIMessage : "Missing keys";
        EventSystem<InteractTriggerEnter>.FireEvent(new InteractTriggerEnter(UIMessage));
    }

    protected override void InsideTrigger() {
        if (keysAcquired.Count == keyList.Count && Input.GetKeyDown(KeyCode.F)) {
            UnlockEvent ue = new UnlockEvent();
            EventSystem<UnlockEvent>.FireEvent(ue);
            EventSystem<InteractTriggerExit>.FireEvent(new InteractTriggerExit());
            Destroy(this);
        }
    }

    protected override void ExitTrigger() {
        EventSystem<InteractTriggerExit>.FireEvent(new InteractTriggerExit());
    }
    

}

