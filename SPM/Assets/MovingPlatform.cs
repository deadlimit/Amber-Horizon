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

    public GameObject left, right;
    

    // Start is called before the first frame update
    void Awake()
    {
        coll = GetComponent<BoxCollider>();

        startPos = transform.position;
        gravity = 0f;

        Vector3 forw = new Vector3(transform.forward.x, 0, transform.forward.y);
        leftMax = -forw.normalized * possibleMoveLength;
        rightMax = forw.normalized * possibleMoveLength;
        left.transform.position = leftMax;
        right.transform.position = rightMax;
    }

    public void Update() {
        
        base.Update();
        
        MovePlatform();
       /* if (Vector3.Distance(transform.position, startPos) + skinWidth - coll.size.z / 2 >= possibleMoveLength)
        {
            Vector3 temp = velocity;
            velocity = Vector3.zero;
            Vector3 normalForce = General.NormalForce3D(temp * 2, -temp);
            velocity += normalForce;
            Debug.Log("normalForce" + normalForce);
        }*/

        /*Vector3 newVel = Vector3.zero;
        newVel.x = Mathf.Clamp(velocity.x, leftMax.x, rightMax.x);
        newVel.z = Mathf.Clamp(velocity.z, leftMax.z, rightMax.z);*/


        transform.position += velocity * Time.deltaTime;
        MoveOutOfGeometry();

    }
    private bool MoveBackAllowed() 
    {
        return transform.position.x < leftMax.x && transform.position.z < leftMax.z;
    }
    private bool MoveForwardAllowed()
    {
        return transform.position.x < rightMax.x && transform.position.z < rightMax.z;
    }
    private void MovePlatform() 
    {
        if (dotProduct < minDistanceForMovement)
            velocity += 0.5f * (movement * -transform.forward * Time.deltaTime);
        else if (dotProduct > minDistanceForMovement)
            velocity += 2f *(movement * transform.forward * Time.deltaTime);
        
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        
        //movement = 0f;
    }
    public void BlackHoleGravity(BlackHole bh)
    {
        Vector3 direction = bh.transform.position - transform.position;
        dotProduct = Vector3.Dot(transform.forward.normalized, bh.transform.position.normalized - transform.position.normalized);
       // Debug.Log(dotProduct);
        Debug.DrawLine(transform.position,transform.position + direction, Color.green);
        bhGrav = bh.gravitationalPull * (bh.transform.position - transform.position) / Mathf.Pow(Vector3.Distance(bh.transform.position, transform.position), 2) * Time.deltaTime;
        movement += new Vector3(bhGrav.x, 0, bhGrav.z).magnitude;
        //velocity += new Vector3(bhGrav.x, 0, bhGrav.z);
        ApplyFriction(General.NormalForce3D(velocity, bh.transform.position - transform.position));
        bhGrav = Vector3.zero;
    }
    public void StopVelocity()
    {
        //vill man bara att detta kallas en gång? 
        //detta hindrar inte att gravitation appliceras


        //gravityMod assignment måste just nu appliceras kontinuerligt, oavsett hur man vill göra med velocity
    }
    //bara kunna röra sig fram och bakåt
    /* Vectror3
     
     
     
     */

}
