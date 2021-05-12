using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class PlayerDiedListener : MonoBehaviour
{
    CheckpointManager checkpointManager;

    private void OnEnable()
    {
        EventSystem<PlayerDiedEvent>.RegisterListener(OnPlayerDeath);
    }

    private void OnDisable() => EventSystem<PlayerDiedEvent>.UnregisterListener(OnPlayerDeath);

    public void OnPlayerDeath(PlayerDiedEvent pde)
    {
        Debug.Log("Player died");
        pde.player.GetComponent<Animator>().StopPlayback();
        pde.player.physics.velocity = Vector3.zero;
        Checkpoint.ActiveCheckPoint.ResetPlayerPosition();
        pde.player.RestoreHealth();
    }
}
