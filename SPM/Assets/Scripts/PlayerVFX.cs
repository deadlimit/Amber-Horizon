using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    public GameObject position;
    public GameObject DashEffect;


    public void PlayDashStart()
    {
        Instantiate(DashEffect, position.transform.position, position.transform.rotation, this.gameObject.transform);
    }
}
