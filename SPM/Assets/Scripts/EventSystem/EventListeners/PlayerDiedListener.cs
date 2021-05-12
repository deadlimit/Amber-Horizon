using UnityEngine;
using EventCallbacks;

public class PlayerDiedListener : MonoBehaviour
{
    private void OnEnable() => EventSystem<PlayerDiedEvent>.RegisterListener(OnPlayerDeath);
    private void OnDisable() => EventSystem<PlayerDiedEvent>.UnregisterListener(OnPlayerDeath);

    public void OnPlayerDeath(PlayerDiedEvent pde)
    {
        pde.player.animator.StopPlayback();
        pde.player.physics.velocity = Vector3.zero;

        GameObject fadeUI = GameObject.Find("FadeToBlack");
        StartCoroutine(fadeUI.GetComponent<DeathFade>().FadeOut());
    }
}
