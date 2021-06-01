using Cinemachine;
using EventCallbacks;
using UnityEngine;

public class EndGateCinematicController : MonoBehaviour {

    private CinemachineTriggerAction cinematicTrigger;

    private void Awake() {
        cinematicTrigger = GetComponent<CinemachineTriggerAction>();
        cinematicTrigger.enabled = false;
    }
    
    private void OnEnable() {
        EventSystem<UnlockEvent>.RegisterListener(EnableEndCinematic);
    }

    private void OnDisable() {
        EventSystem<UnlockEvent>.UnregisterListener(EnableEndCinematic);
    }

    private void EnableEndCinematic(UnlockEvent unlockEvent) {
        if (unlockEvent.ID != 3) return;

        print("hehe");
        cinematicTrigger.enabled = true;
    }
}
