using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public LayerMask physicsLayerMask;
    public LayerMask collisionMask;
    public float gravitationalPull;
    public float gravity = 10f;
    public Vector3 velocity;
    public float airResistance = 0f;
    public float skinWidth;

    SphereCollider coll;
    BoxCollider centerColl;

    private bool useGravity = true;
    private float terminalDistance = 0.5f;

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
            if (c.GetComponent<PhysicsComponent>())
            {
                if (Vector3.Distance(transform.position, c.transform.position) < terminalDistance)
                    c.GetComponent<PhysicsComponent>().StopVelocity();

                else
                    c.GetComponent<PhysicsComponent>().BlackHoleGravity(this);
            }

        }
        //---------------------------------------------------------

        //om man sätter if(useGravity) här ute kan vi nog slippa göra boxcasten när hålet ändå står still? 

        /*
        RaycastHit hitInfo;

        if (Physics.BoxCast(transform.position, centerColl.size / 2, velocity.normalized, out hitInfo, Quaternion.identity, (velocity.magnitude * Time.deltaTime + skinWidth), collisionMask))
            {
                Debug.Log("collision layer");
            //kanske vill ha liiiite fysik i träffen
                velocity = Vector3.zero;
                useGravity = false;
            }*/
        if (useGravity)
        {
            //verkar som att overlapbox är mycket mindre benägen att gå igenom väggar.
            Collider[] boxHitColl =
            Physics.OverlapBox(transform.position, centerColl.size / 2, Quaternion.identity, collisionMask);
            if (boxHitColl.Length > 0)
            {
                velocity = Vector3.zero;
                useGravity = false;
            }

            velocity += Vector3.down * Time.deltaTime * gravity;
        }

        velocity *= Mathf.Pow(airResistance, Time.deltaTime);
        transform.Translate(velocity * Time.deltaTime);
    }
}
