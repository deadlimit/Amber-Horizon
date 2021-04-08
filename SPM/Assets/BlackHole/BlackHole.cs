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

    public static float BlackHoleRadius;
    
    
    SphereCollider coll;
    BoxCollider centerColl;

    private bool useGravity = true;
    private float terminalDistance = 0.5f;

    private void Awake() {
        
        coll = GetComponent<SphereCollider>();
        BlackHoleRadius = coll.radius;
        centerColl = GetComponent<BoxCollider>();
    }
    // Update is called once per frame
    void Update()
    {
        GravitationDrag();
        //CheckCenterCollision();
        
        velocity *= Mathf.Pow(airResistance, Time.deltaTime);
        transform.Translate(velocity * Time.deltaTime);
    }
    private void GravitationDrag()
    {
        Collider[] hitcoll = Physics.OverlapSphere(transform.position, coll.radius, physicsLayerMask);
        foreach (Collider c in hitcoll)
        {

            //fysikp�verkan
            if (c.GetComponent<PhysicsComponent>()) {
                c.GetComponent<PhysicsComponent>().AffectedByBlackHoleGravity = true;
                if (Vector3.Distance(transform.position, c.transform.position) < terminalDistance)
                    c.GetComponent<PhysicsComponent>().StopVelocity();

                else {
                    IBlackHoleBehaviour blackHoleBehaviour = c.GetComponent<IBlackHoleBehaviour>();
                    c.GetComponent<PhysicsComponent>().BlackHoleGravity(this, blackHoleBehaviour);
                }
                    
            }

        }
        //---------------------------------------------------------

        //om man s�tter if(useGravity) h�r ute kan vi nog slippa g�ra boxcasten n�r h�let �nd� st�r still? 

        /*
        RaycastHit hitInfo;

        if (Physics.BoxCast(transform.position, centerColl.size / 2, velocity.normalized, out hitInfo, Quaternion.identity, (velocity.magnitude * Time.deltaTime + skinWidth), collisionMask))
            {
                Debug.Log("collision layer");
            //kanske vill ha liiiite fysik i tr�ffen
                velocity = Vector3.zero;
                useGravity = false;
            }*/
        if (useGravity)
        {
            //verkar som att overlapbox �r mycket mindre ben�gen att g� igenom v�ggar.
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
