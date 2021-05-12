using UnityEngine;
using EventCallbacks;
using System.Collections.Generic;

public class PlayerReviveListener : MonoBehaviour
{
    public static List<Enemy> enemyList = new List<Enemy>();
    private void OnEnable() => EventSystem<PlayerReviveEvent>.RegisterListener(PlayerRevive);
    private void OnDisable() => EventSystem<PlayerReviveEvent>.UnregisterListener(PlayerRevive);

    public void PlayerRevive(PlayerReviveEvent pre)
    {
        ResetEnemyPositions();
        pre.player.animator.StopPlayback();
        Checkpoint.ActiveCheckPoint.ResetPlayerPosition();
        pre.player.RestoreHealth();
    }
    private void ResetEnemyPositions()
    {
        foreach(Enemy e in enemyList)
        {
            Debug.Log(e.gameObject);
            e.ResetPosition();
        }
    }
}

