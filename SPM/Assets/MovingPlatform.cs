using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : PhysicsComponent
{
    Vector3 startPos;
    Vector3 leftMax, rightMax;
    public Vector3 vel;
    public float maxSpeed;
    BoxCollider coll;
    [SerializeField] float possibleMoveLength;
    [SerializeField] float minDistanceForMovement = 0.02f;
    float movement = 0f;
    float dotProduct;
    

    // Start is called before the first frame update
    void Awake()
    {
        coll = GetComponent<BoxCollider>();

        startPos = transform.position;
        gravity = 0f;

        leftMax = -transform.forward * possibleMoveLength;
        rightMax = transform.forward * possibleMoveLength;
    }

    public void Update()
    {
        vel = velocity;
        bhGrav = Vector3.zero;
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
        AddGravity();

        CheckForCollisions(0);

        if (Vector3.Distance(transform.position, startPos) + skinWidth - coll.size.z / 2 >= possibleMoveLength)
        {
            Vector3 temp = velocity;
            velocity = Vector3.zero;
            Vector3 normalForce = General.NormalForce3D(temp, -temp.normalized);
            velocity += normalForce;
            Debug.Log(normalForce);
        }

        else
        {
            transform.position += velocity * Time.deltaTime;
        }





        MoveOutOfGeometry();
        MovePlatform();
        //OverlapCapsule();
    }

    private void MovePlatform() 
    {
        if (dotProduct < minDistanceForMovement)
            velocity += 0.5f *(movement * -transform.forward * Time.deltaTime);
        else if (dotProduct > minDistanceForMovement)
            velocity += 1f *(movement * transform.forward * Time.deltaTime);
        
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        
        //movement = 0f;
    }
    public override void BlackHoleGravity(BlackHole bh)
    {
        Vector3 direction = bh.transform.position - transform.position;
        dotProduct = Vector3.Dot(transform.forward.normalized, bh.transform.position.normalized - transform.position.normalized);
        Debug.Log(dotProduct);
        Debug.DrawLine(transform.position,transform.position + direction, Color.green);
        bhGrav = bh.gravitationalPull * (bh.transform.position - transform.position) / Mathf.Pow(Vector3.Distance(bh.transform.position, transform.position), 2) * Time.deltaTime;
        movement += new Vector3(bhGrav.x, 0, bhGrav.z).magnitude;
        //velocity += new Vector3(bhGrav.x, 0, bhGrav.z);
        ApplyFriction(General.NormalForce3D(velocity, bh.transform.position - transform.position));
        bhGrav = Vector3.zero;
    }

    //bara kunna röra sig fram och bakåt
    /* Vectror3
     
     
     
     */

}
