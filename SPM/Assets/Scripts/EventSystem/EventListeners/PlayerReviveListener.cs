using UnityEngine;
using EventCallbacks;

public class PlayerReviveListener : MonoBehaviour
{
    private void OnEnable() => EventSystem<PlayerReviveEvent>.RegisterListener(PlayerRevive);
    private void OnDisable() => EventSystem<PlayerReviveEvent>.UnregisterListener(PlayerRevive);

    public void PlayerRevive(PlayerReviveEvent pre)
    {
        pre.player.animator.StopPlayback();
        Checkpoint.ActiveCheckPoint.ResetPlayerPosition();
        pre.player.RestoreHealth();
    }
}

