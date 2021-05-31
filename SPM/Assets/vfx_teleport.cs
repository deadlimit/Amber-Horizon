using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vfx_teleport : MonoBehaviour
{
    public GameObject VFXEnd;
    public GameObject VFX;
    public GameObject position;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayTeleportVFX()
    {
        Instantiate(VFX, position.transform.position, position.transform.rotation, transform);
    }

    public void PlayTeleportVFXEnd()
    {
        Instantiate(VFXEnd, position.transform.position, position.transform.rotation, transform);
    }
}
