using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class PlatformReseter : MonoBehaviour
{
    [SerializeField]
    private GameObject platformPrefab;

    private void Awake()
    {
        EventSystem<PlayerReviveEvent>.RegisterListener(ResetPlatform);
    }

    private void ResetPlatform(PlayerReviveEvent playerReviveEvent)
    {
        //idk
        //find child with tag "physics component". 
        //GetC

        Destroy(GetComponentInChildren<MovingPlatform>().gameObject);

        //spawn a new platform.

        Instantiate(platformPrefab, gameObject.transform, false);

    }
}
