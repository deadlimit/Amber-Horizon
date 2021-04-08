using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour {

    private Dictionary<int, Checkpoint> checkpoints = new Dictionary<int, Checkpoint>();

    private Checkpoint activeCheckpointPosition;
    
    private void Awake() {
        
        //Hämta alla gameobjects med Checkpoints-tag
        List<GameObject> checkpointsInScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("Checkpoint"));
        
        List<Checkpoint> sortedCheckpoints = new List<Checkpoint>(checkpointsInScene.Count);
        
        
        //Hämta Checkpoint-komponenten hos varje gameobject och lägg till den i sortedCheckpoints
        checkpointsInScene.ForEach(checkpoint => {
            sortedCheckpoints.Add(checkpoint.GetComponent<Checkpoint>());
            checkpoint.GetComponent<Checkpoint>().OnPlayerEnter += UpdateCheckPoint;
        });
        
        /*
        //Sortera alla Checkpoints i stigande ordning efter order-numret
        sortedCheckpoints.Sort((checkpoint, checkpoint1) => checkpoint1.order.CompareTo(checkpoint.order));
        */
        //Flytta sorterade värden till en stack.
        sortedCheckpoints.ForEach(checkpoint => checkpoints.Add(checkpoint.ID, checkpoint));
        
    }

    private void UpdateCheckPoint(int ID) {
        Checkpoint point = checkpoints[ID];
        
        if(activeCheckpointPosition)
            activeCheckpointPosition.gameObject.SetActive(true);
        
        activeCheckpointPosition = point;
        
        activeCheckpointPosition.gameObject.SetActive(false);
    }

}
