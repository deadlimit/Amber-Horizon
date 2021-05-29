using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Destroyer : PoolObject
{

    public float timer = 5;
    private float time = 0;

    void Update()
    {
        time += Time.deltaTime;

        if(time > timer)
            gameObject.SetActive(false);


    }
}
