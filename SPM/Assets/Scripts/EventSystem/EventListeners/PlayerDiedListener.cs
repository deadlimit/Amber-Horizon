using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class PlayerDiedListener : MonoBehaviour
{
    CheckpointManager checkpointManager;
    Animator playerAnim;
    private void OnEnable()
    {
        EventSystem<PlayerDiedEvent>.RegisterListener(OnPlayerDeath);
        checkpointManager = GameObject.FindGameObjectWithTag("CheckpointManager").GetComponent<CheckpointManager>();
    }

    private void OnDisable() => EventSystem<PlayerDiedEvent>.UnregisterListener(OnPlayerDeath);

    public void OnPlayerDeath(PlayerDiedEvent pde)
    {
        Debug.Log("Player died");
        pde.player.GetComponent<Animator>().StopPlayback();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PhysicsComponent>().velocity = Vector3.zero;
        checkpointManager.ResetPlayerPosition();
        pde.player.RestoreHealth();
    }
}
