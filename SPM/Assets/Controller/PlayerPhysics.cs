using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    public LayerMask collisionMask;
    public Camera cam;
    public float fallSpeed = 1f;
    public RaycastHit groundHitInfo;

    [SerializeField] public Vector3 velocity { get; set; }
    [SerializeField] public float gravity =  10f;
    [Range(0f, 1f)] [SerializeField] float staticFrictionCoefficient = 0.5f;
    [Range(0f, 1f)] [SerializeField] float kineticFrictionCoefficient = 0.35f;
    [Range(0f, 1f)] [SerializeField] float airResistance = 0.35f;
    [SerializeField] float skinWidth = 0.05f;

    float smallNumber = 0.05f;
    float groundCheckDistance = 0.05f;
    float gravityMod = 1f;
    Vector3 bhGrav = Vector3.zero;

    Vector3 point1 = Vector3.zero;
    Vector3 point2 = Vector3.zero;
    Vector3 fallVec = Vector3.zero;
    CapsuleCollider capColl;

    void Start()
    {
        capColl = GetComponent<CapsuleCollider>();
    }

    private void CapsuleCollision() 
    {
        point1 = (transform.position + capColl.center) + Vector3.up * capColl.radius;
        point2 = (transform.position + capColl.center) + Vector3.down * capColl.radius;

        RaycastHit hitInfo;
        RaycastHit normalHit;

        if (velocity.magnitude < smallNumber)
            velocity = Vector2.zero;

        if (Physics.CapsuleCast(point1, point2, capColl.radius, velocity.normalized,
            out hitInfo, velocity.magnitude * Time.deltaTime + skinWidth, collisionMask))
        {
            Physics.CapsuleCast(point1, point2, capColl.radius, -hitInfo.normal,
                out normalHit, (velocity.magnitude + capColl.radius) * Time.deltaTime + skinWidth, collisionMask);

            velocity += -normalHit.normal * (normalHit.distance - skinWidth);

            //hör applicerar vi "stopp"-kraften från ytan vi kolliderar med
            Vector3 normalForce = General.NormalForce3D(velocity, normalHit.normal);
            velocity += normalForce;
            ApplyFriction(normalForce);
        }
    }

    private void OverlapCapsule()
        {
        point1 = (transform.position + capColl.center) + Vector3.up * capColl.radius;
        point2 = (transform.position + capColl.center) + Vector3.down * capColl.radius;

        Collider[] hitColl = Physics.OverlapCapsule(point1, point2, capColl.radius, collisionMask);
            if (hitColl.Length != 0)
            {
                //Debug.Log("Overlap träff");
                foreach (Collider c in hitColl)
                {
                    Physics.ComputePenetration(capColl, transform.position, transform.rotation,
                        c, c.transform.position, c.transform.rotation,
                        out Vector3 separationDirection, out float hitDistance);

                    Vector3 separationVector = separationDirection * hitDistance;
                    transform.position += separationVector + separationVector.normalized * skinWidth;
                    velocity += General.NormalForce3D(velocity, separationVector.normalized);
                    ApplyFriction(General.NormalForce3D(velocity, separationVector.normalized));
                }
            }
        }
    

    public void HandleInput(Vector3 inp) {
        bhGrav = Vector3.zero;
        //isGrounded är med för att uppdatera groundHitInfo
        Debug.DrawLine(transform.position, transform.position + velocity, Color.red);
   
        velocity += inp;
        //isGrounded();
        AddGravity();

        CapsuleCollision();
        transform.position += velocity * Time.deltaTime;
        OverlapCapsule();

    }
    public bool isGrounded() {
        return Physics.CapsuleCast(point1, point2, capColl.radius, Vector3.down, out groundHitInfo, groundCheckDistance + skinWidth, collisionMask); }

    public Vector3 GetVelocity() { return velocity; }
    private void AddGravity()
    {    
        Vector3 gravityMovement = gravity * Vector3.down * Time.deltaTime * gravityMod;

        if (bhGrav != Vector3.zero)
        {
            gravityMovement = bhGrav;
            velocity *= 0.05f;
            
        }

        velocity += gravityMovement;
        gravityMod = 1f;
    }
    private void ApplyFriction(Vector3 normalForce)
    {
        if (velocity.magnitude < normalForce.magnitude * staticFrictionCoefficient)
            velocity = Vector2.zero;
        else
        {
            velocity -= velocity.normalized * normalForce.magnitude * kineticFrictionCoefficient;
        }
        velocity *= Mathf.Pow(airResistance, Time.deltaTime);
    }
    public void AddFallSpeed() {}

    public void BlackHoleGravity(BlackHole bh) 
    {
        bhGrav = bh.gravitationalPull * (bh.transform.position - transform.position) / Mathf.Pow(Vector3.Distance(bh.transform.position, transform.position), 2) * Time.deltaTime;
        velocity += bhGrav;
        ApplyFriction(General.NormalForce3D(velocity, bh.transform.position - transform.position));
        bhGrav = Vector3.zero;
    }
    public void StopVelocity() 
    {
        //vill man bara att detta kallas en gång? 
        //detta hindrar inte att gravitation appliceras
        velocity -= velocity *0.02f;
        
        //gravityMod assignment måste just nu appliceras kontinuerligt, oavsett hur man vill göra med velocity
        gravityMod = 0.1f;
    }

}
