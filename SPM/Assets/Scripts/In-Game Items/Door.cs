using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField ] private BoxCollider collisionColl;
    private BoxCollider coll;

    private void Awake()
    {
        coll = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //animation
            collisionColl.enabled = false;
        }
    }
    public void Unlock() { coll.enabled = true; }


}
