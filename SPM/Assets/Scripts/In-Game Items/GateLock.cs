using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
public class GateLock : InteractableObject
{
    public static List<KeyFragment> KeyList = new List<KeyFragment>();
    public static List<KeyFragment> KeysAcquired = new List<KeyFragment>();
    
    [SerializeField] private int doorIDToOpen;
    
    protected BoxCollider interaction { get; private set; }

    [Header("Fuskknapp, sätt till true för att gaten ska öppna direkt (debug)")]
    [SerializeField] protected bool OpenDoorWithoutKeys;

    private void OnEnable() {
        EventSystem<KeyPickUpEvent>.RegisterListener(KeyPickUp);
        EventSystem<NewLevelLoadedEvent>.RegisterListener(ResetKeys);
    }
    
    private void OnDisable() {
        EventSystem<KeyPickUpEvent>.UnregisterListener(KeyPickUp);
        EventSystem<NewLevelLoadedEvent>.UnregisterListener(ResetKeys);
    }

    private void Awake() {
        interaction = GetComponent<BoxCollider>();
        
    }
    
    private void Start() {

        ResetKeys(null);

        if (!OpenDoorWithoutKeys) return;
        
        UnlockGateSequence();

    }
    
    private void KeyPickUp(KeyPickUpEvent kpue)
    {
        Debug.Log("in GateLock, KeyPickUp. KeyList is: " + KeyList.Count + ". KeysAcquired is: " + KeysAcquired.Count);
        if (KeyList.Count == KeysAcquired.Count || KeyList.Count - 1 == KeysAcquired.Count)
        {
            Debug.Log("in GateLock. interaction enabled.");
            interaction.enabled = true;
        }
    }
    
    protected override void EnterTrigger(string UIMessage) {

        if (KeyList.Count == KeysAcquired.Count || KeyList.Count - 1 == KeysAcquired.Count)
        {
            EventSystem<InteractTriggerEnterEvent>.FireEvent(new InteractTriggerEnterEvent(UIMessage));

        }
        //else keep the UIMessage as is.
        //UIMessage = KeyList.Count == KeysAcquired.Count ? UIMessage : "Missing key fragments: " + (KeyList.Count - KeysAcquired.Count);
        else 
        {
            UIMessage = "Missing key fragments: " + (KeyList.Count - 1 - KeysAcquired.Count);
            EventSystem<InteractTriggerEnterEvent>.FireEvent(new InteractTriggerEnterEvent(UIMessage));
        }
    }

    protected override void InsideTrigger(GameObject player) {
        
        if ((KeyList.Count == KeysAcquired.Count || KeyList.Count - 1 == KeysAcquired.Count) && Input.GetKeyDown(KeyCode.E)) {
            FireUnlockSequence();
        }
    }

    protected override void ExitTrigger() {
        EventSystem<InteractTriggerExitEvent>.FireEvent(new InteractTriggerExitEvent());
    }

    private void UnlockGateSequence() {
        UnlockEvent ue = new UnlockEvent(doorIDToOpen);
        EventSystem<UnlockEvent>.FireEvent(ue);
        EventSystem<InteractTriggerExitEvent>.FireEvent(new InteractTriggerExitEvent());
        interaction.enabled = false;
        Destroy(this);
    }

    protected virtual void FireUnlockSequence() {
        UnlockGateSequence();
        Debug.Log("in GateLock. in FireUnlockSequence.");
    }

    private void ResetKeys(NewLevelLoadedEvent loadedEvent) {
        
        KeyList.Clear();
        KeysAcquired.Clear();
        
        KeyFragment[] keys = FindObjectsOfType<KeyFragment>();
        
        foreach(KeyFragment key in keys)
            KeyList.Add(key);
        EventSystem<KeyPickUpEvent>.FireEvent(null);

    }
}

