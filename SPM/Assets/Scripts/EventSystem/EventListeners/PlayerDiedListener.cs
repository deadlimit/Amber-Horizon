using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class PlayerDiedListener : MonoBehaviour
{
    Animator playerAnim;
    private void OnEnable()
    {
        EventSystem<PlayerDiedEvent>.RegisterListener(OnPlayerDeath);
    }

    private void OnDisable() => EventSystem<PlayerDiedEvent>.UnregisterListener(OnPlayerDeath);

    public void OnPlayerDeath(PlayerDiedEvent pde)
    {
        pde.player.GetComponent<Animator>().StopPlayback();
        GameObject.FindGameObjectWithTag("Player").GetComponent<PhysicsComponent>().velocity = Vector3.zero;
        Checkpoint.ActiveCheckPoint.ResetPlayerPosition();
        pde.player.RestoreHealth();
    }
}
