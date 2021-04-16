using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFragment : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Controller3D player = other.gameObject.GetComponent<Controller3D>();
        if (player)
        {
            player.AddKeyFragment();
            Destroy(gameObject);
        }
    }

    
}
