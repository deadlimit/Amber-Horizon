using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    private Dictionary<int, Checkpoint> checkpoints = new Dictionary<int, Checkpoint>();

    public Checkpoint startCheckpoint;
    
    private Checkpoint activeCheckpointPosition;

    private Transform player;

    private static int ID = 0;
    
    private void Awake() {

        player = GameObject.FindGameObjectWithTag("Player").transform;

        activeCheckpointPosition = startCheckpoint;

        //Hämta alla gameobjects med Checkpoints-tag
        List<GameObject> checkpointsInScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("Checkpoint"));
        
        //Loopa igenom alla gameobject, hämta Checkpoint-komponenten och subscribe till checkpointens event
        //lägg sedan till checkpointen i hashtabellen
        checkpointsInScene.ForEach(checkpoint => {
            
            Checkpoint checkPoint = checkpoint.GetComponent<Checkpoint>();

           // checkPoint.OnPlayerEnter += UpdateCheckPoint(ID+1);
            
            checkpoints.Add(checkPoint.ID, checkPoint);
            
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
        player.position = activeCheckpointPosition.transform.position;
    }

}
