using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    private Dictionary<int, Checkpoint> checkpoints = new Dictionary<int, Checkpoint>();
    
    private Checkpoint activeCheckpointPosition;

    private Transform player;

    private static int NextID = 0;
    
    private void Awake() {

        player = GameObject.FindGameObjectWithTag("Player").transform;

        float currentMinimumDistance = int.MaxValue;
        
        //Hämta alla gameobjects med Checkpoints-tag
        List<GameObject> checkpointsInScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("Checkpoint"));
        
        //Loopa igenom alla gameobject, hämta Checkpoint-komponenten och subscribe till checkpointens event
        //lägg sedan till checkpointen i hashtabellen
        checkpointsInScene.ForEach(checkpoint => {

            Checkpoint currentCheckpoint = checkpoint.GetComponent<Checkpoint>();

            currentCheckpoint.ID = NextID++;

            currentCheckpoint.OnPlayerEnter += UpdateCheckPoint;
            
            checkpoints.Add(currentCheckpoint.ID, currentCheckpoint);

            float distanceToPlayer = Vector3.Distance(checkpoint.transform.position, player.transform.position);

            if (distanceToPlayer < currentMinimumDistance) 
                activeCheckpointPosition = currentCheckpoint;
            
        });
        
    }
    
    private void UpdateCheckPoint(int ID) {
        Checkpoint point = checkpoints[ID];
        
        if(activeCheckpointPosition)
            activeCheckpointPosition.gameObject.SetActive(true);
        
        activeCheckpointPosition = point;
        
        activeCheckpointPosition.gameObject.SetActive(false);
    }

    public void ResetPlayerPosition() {
        player.position = activeCheckpointPosition.SpawnPosition;
    }

}
