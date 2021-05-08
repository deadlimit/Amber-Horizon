using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    private Dictionary<int, Checkpoint> checkpoints = new Dictionary<int, Checkpoint>();
    
    private Checkpoint activeCheckpointPosition;

    private Transform player;

    private static int NextID = 0;

    private void OnEnable() => EventSystem<CheckPointActivatedEvent>.RegisterListener(UpdateCheckPoint);

    private void OnDisable() => EventSystem<CheckPointActivatedEvent>.UnregisterListener(UpdateCheckPoint);

    //TODO Custom-inspektor för att kunna välja om man vill assigna en referens till startcheckpoint eller om scriptet ska sköta det själv. 
    private void Awake() {

        player = GameObject.FindGameObjectWithTag("Player").transform;

        float currentMinimumDistance = int.MaxValue;
        Checkpoint closestCheckpoint = null;
        //Hämta alla gameobjects med Checkpoints-tag
        List<GameObject> checkpointsInScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("Checkpoint"));
        
        //Loopa igenom alla gameobject, hämta Checkpoint-komponenten och subscribe till checkpointens event
        //lägg sedan till checkpointen i hashtabellen
        checkpointsInScene.ForEach(checkpoint => {

            Checkpoint currentCheckpoint = checkpoint.GetComponent<Checkpoint>();

            currentCheckpoint.ID = NextID++;
            
            checkpoints.Add(currentCheckpoint.ID, currentCheckpoint);

            float distanceToPlayer = Vector3.Distance(player.transform.position, currentCheckpoint.transform.position);
            
            if (distanceToPlayer < currentMinimumDistance) {
                closestCheckpoint = currentCheckpoint;
                currentMinimumDistance = distanceToPlayer;
            }
            
        });
        
        activeCheckpointPosition = closestCheckpoint;
        
    }
    
    private void UpdateCheckPoint(CheckPointActivatedEvent checkPointActivatedEvent) {
        Checkpoint point = checkpoints[checkPointActivatedEvent.ID];
        //här sätts activecheckpoint till röd
        activeCheckpointPosition.ChangeParticleColor(false);
        if(activeCheckpointPosition)
            activeCheckpointPosition.gameObject.SetActive(true);
        
        activeCheckpointPosition = point;
        //här sätts den nya activecheckpoint till grön
        activeCheckpointPosition.ChangeParticleColor(true);

        activeCheckpointPosition.gameObject.SetActive(false);
    }

    public void ResetPlayerPosition() {
        player.position = activeCheckpointPosition.SpawnPosition;
    }

}
