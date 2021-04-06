using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public LayerMask collisionMask;
    SphereCollider coll;
    public float bhGravity;
    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
    }
    // Update is called once per frame
    void Update()
    {
        Collider [] hitcoll = Physics.OverlapSphere(transform.position, coll.radius, collisionMask);
        foreach(Collider c in hitcoll)
        {
            c.GetComponent<PlayerPhysics>().BlackHoleGravity(gameObject);
        }
    }
}
