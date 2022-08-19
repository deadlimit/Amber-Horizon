using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class PlayerVFX : MonoBehaviour
{
    public GameObject position;
    public GameObject DashEffect;

    public GameObject RingEffect;

    public void Start()
    {
        EventSystem<PlayerReviveEvent>.RegisterListener(PlayReviveEffect);
    }

    public void PlayDashStart()
    {
        Instantiate(DashEffect, position.transform.position, position.transform.rotation, this.gameObject.transform);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup")) 
        {
            Instantiate(RingEffect, this.transform.position, this.transform.rotation, this.gameObject.transform);
        }
    }

    public void CallsTeleportEffect()
    {
        //needs a delay so the effect happens after the camera move.
        Debug.Log("In PlayerVFX. start of telportvfx");
        StartCoroutine(PlayTeleportEffect(0.4f));
        
        Debug.Log("In PlayerVFX. end of telportvfx");
    }

    private void PlayReviveEffect(PlayerReviveEvent playerReviveEvent)
    {
        Debug.Log("In PlayerVFX. start of revive.");
        StartCoroutine(PlayTeleportEffect(0.2f));
    }

    private IEnumerator PlayTeleportEffect(float delay) 
    {
        yield return new WaitForSeconds(delay);

        Instantiate(RingEffect, this.transform.position, this.transform.rotation, this.gameObject.transform);
    }

}
