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
        if (!other.CompareTag("Pickup"))
            return;

        Instantiate(RingEffect, this.transform.position, this.transform.rotation, this.gameObject.transform);
    }
}
