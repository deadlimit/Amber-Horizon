using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int hitsBeforeDeath;

    private CheckpointManager checkpointManager;
    private int hitsBeforeDeathDefault;
    private void Awake() {
        hitsBeforeDeathDefault = hitsBeforeDeath;
        checkpointManager = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();
    }

    public void TakeDamage() {
        --hitsBeforeDeath;

        if (hitsBeforeDeath < 1) {
            Checkpoint.ActiveCheckPoint.ResetPlayerPosition();
            hitsBeforeDeath = hitsBeforeDeathDefault;
        }
    }

}
