using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private BoxCollider coll;
    private BoxCollider collisionColl;

    private void Awake()
    {
        coll = GetComponent<BoxCollider>();
        collisionColl = transform.GetChild(0).GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Debug.Log("Dörren öppnas med animation och borttagen collider");
            //animation
            collisionColl.enabled = false;
        }
    }
    public void Unlock() { coll.enabled = true; }


}
