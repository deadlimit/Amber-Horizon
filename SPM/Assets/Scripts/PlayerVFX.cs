using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{

    public GameObject DashEffectStart;
    public GameObject DashEffectEnd;
    public GameObject position;
    public GameObject DashEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDashStart()
    {
        Instantiate(DashEffect, position.transform.position, position.transform.rotation, position.transform);
    }

    public void PlayDashEnd()
    {
        Instantiate(DashEffectEnd, position.transform.position, position.transform.rotation, position.transform);
    }
}
