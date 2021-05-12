using EventCallbacks;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

 /*   [SerializeField] private Checkpoint activeCheckpoint;

    private Transform player;
    
    private void OnEnable() => EventSystem<CheckPointActivatedEvent>.RegisterListener(UpdateCheckPoint);

    private void OnDisable() => EventSystem<CheckPointActivatedEvent>.UnregisterListener(UpdateCheckPoint);

    //TODO Custom-inspektor för att kunna välja om man vill assigna en referens till startcheckpoint eller om scriptet ska sköta det själv. 
    
    private void UpdateCheckPoint(CheckPointActivatedEvent checkPointActivatedEvent) {
        
        Checkpoint point = checkPointActivatedEvent.checkpoint;
        //här sätts activecheckpoint till röd
        activeCheckpoint.ChangeParticleColor(false);
        if(activeCheckpoint)
            activeCheckpoint.gameObject.SetActive(true);
        
        activeCheckpoint = point;
        //här sätts den nya activecheckpoint till grön
        activeCheckpoint.ChangeParticleColor(true);
        
        activeCheckpoint.gameObject.SetActive(false);
    }

    public void ResetPlayerPosition() {
        player.position = activeCheckpoint.SpawnPosition;
    }
*/
}
