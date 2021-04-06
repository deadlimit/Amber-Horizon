using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public LayerMask physicsLayerMask;
    public LayerMask collisionMask;
    public float gravitationalPull;
    public float gravity = 10f;
    public float skinWidth;
    public Vector3 velocity;
    SphereCollider coll;
    BoxCollider centerColl;

    private bool useGravity = true;
    float terminalDistance = 0.5f;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
        centerColl = GetComponent<BoxCollider>();
    }
    // Update is called once per frame
    void Update()
    {
        Collider [] hitcoll = Physics.OverlapSphere(transform.position, coll.radius, physicsLayerMask);
        foreach(Collider c in hitcoll)
        {
      //-----------------------------------------------------
      //fysikpåverkan
            if (c.GetComponent<PlayerPhysics>())
            {
                if (Vector3.Distance(transform.position, c.transform.position) < terminalDistance)
                    c.GetComponent<PlayerPhysics>().StopVelocity();

                else
                    c.GetComponent<PlayerPhysics>().BlackHoleGravity(this);
            }

        }
        //---------------------------------------------------------
        //om man sätter if(useGravity) här ute kan vi nog slippa göra boxcasten när hålet ändå står still? 
        RaycastHit hitInfo;
        if (Physics.BoxCast(transform.position, centerColl.size / 2, velocity.normalized, out hitInfo, Quaternion.identity, (velocity.magnitude * Time.deltaTime + skinWidth), collisionMask))
            {
                Debug.Log("collision layer");
            //kanske vill ha liiiite fysik i träffen
                velocity = Vector3.zero;
                useGravity = false;
            }
       
        if(useGravity)
            velocity += Vector3.down * Time.deltaTime * gravity;

        transform.Translate(velocity * Time.deltaTime);
    }
}
