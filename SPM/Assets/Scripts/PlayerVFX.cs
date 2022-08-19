using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    public GameObject position;
    public GameObject DashEffect;

    public GameObject RingEffect;

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
        StartCoroutine(PlayTeleportEffect());
        
        Debug.Log("In PlayerVFX. end of telportvfx");
    }

    private IEnumerator PlayTeleportEffect() 
    {

        yield return new WaitForSeconds(0.4f);

        Instantiate(RingEffect, this.transform.position, this.transform.rotation, this.gameObject.transform);
    }

}
