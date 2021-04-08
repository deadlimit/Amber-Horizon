using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsComponent : MonoBehaviour
{
    public LayerMask collisionMask;
    public Camera cam;
    public RaycastHit groundHitInfo;
    protected Vector3 bhGrav = Vector3.zero;
    protected float gravityMod = 1f;

    protected Collider attachedCollider;
    private CollisionCaster collisionCaster;

    [SerializeField] public Vector3 velocity;
    [SerializeField] public float gravity = 10f;
    [SerializeField]protected float skinWidth = 0.05f;
    
    [Header ("Friktion")]
    [Range(0f, 1f)] [SerializeField] float staticFrictionCoefficient = 0.5f;
    [Range(0f, 1f)] [SerializeField] float kineticFrictionCoefficient = 0.35f;
    [Range(0f, 1f)] [SerializeField] float airResistance = 0.35f;
    
    private void OnEnable()
    {
        attachedCollider = GetComponent<Collider>();

        if (attachedCollider is BoxCollider)
            collisionCaster = new BoxCaster(attachedCollider, collisionMask);

        if (attachedCollider is SphereCollider)
            collisionCaster = new SphereCaster(attachedCollider, collisionMask);

        if (attachedCollider is CapsuleCollider)
            collisionCaster = new CapsuleCaster(attachedCollider, collisionMask);
    }

    public void Update()
    {
        bhGrav = Vector3.zero;
        AddGravity();
        CheckForCollisions(0);
        transform.position += velocity * Time.deltaTime;
        MoveOutOfGeometry();
    }
    protected void CheckForCollisions(int i)
    {

        RaycastHit hitInfo = collisionCaster.CastCollision(transform.position, velocity.normalized, velocity.magnitude * Time.deltaTime + skinWidth);

        if (!hitInfo.collider) return;

        RaycastHit normalHitInfo = collisionCaster.CastCollision(transform.position, -hitInfo.normal, hitInfo.distance);

        velocity += -normalHitInfo.normal * (normalHitInfo.distance - skinWidth);

        Vector3 normalForce = General.NormalForce3D(velocity, normalHitInfo.normal);
        velocity += normalForce;

        ApplyFriction(normalForce);

        if(i < 10)
            CheckForCollisions(i+1);
    }
    protected void MoveOutOfGeometry()
    {
        Collider[] colliders = collisionCaster.OverlapCast(transform.position);

        if (colliders.Length < 1) return;

        foreach (Collider currentCollider in colliders)
        {
            if (currentCollider == attachedCollider)
                continue;
            Physics.ComputePenetration(attachedCollider,
                                        transform.position,
                                        transform.rotation,
                                        currentCollider,
                                        currentCollider.transform.position,
                                        currentCollider.transform.rotation,
                                        out Vector3 seperationVector,
                                               out float distance);

            Vector3 seperationVec = seperationVector * distance;
            transform.position += seperationVec + seperationVec.normalized * skinWidth;
            velocity += General.NormalForce3D(velocity, seperationVector);
        }

    }
    public void BlackHoleGravity(BlackHole bh, IBlackHoleBehaviour blackHoleBehaviour)
    {
        if (blackHoleBehaviour != null) {
            blackHoleBehaviour.BlackHoleBehaviour(bh);
        }
        else {
            bhGrav = bh.gravitationalPull * (bh.transform.position - transform.position) /
                Mathf.Pow(Vector3.Distance(bh.transform.position, transform.position), 2) * Time.deltaTime;
            velocity += bhGrav;
            ApplyFriction(General.NormalForce3D(velocity, bh.transform.position - transform.position));
            bhGrav = Vector3.zero;
        }
    }
    public void StopVelocity()
    {
        //vill man bara att detta kallas en g�ng? 
        //detta hindrar inte att gravitation appliceras
        velocity -= velocity * 0.02f;

        //gravityMod assignment m�ste just nu appliceras kontinuerligt, oavsett hur man vill g�ra med velocity
        gravityMod = 0.1f;
    }
    protected void AddGravity()
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
    /*private void CapsuleCollision()
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

            //h�r applicerar vi "stopp"-kraften fr�n ytan vi kolliderar med
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
    }*/
    protected void ApplyFriction(Vector3 normalForce)
    {
        if (velocity.magnitude < normalForce.magnitude * staticFrictionCoefficient)
            velocity = Vector2.zero;
        else
        {
            velocity -= velocity.normalized * normalForce.magnitude * kineticFrictionCoefficient;
        }
        velocity *= Mathf.Pow(airResistance, Time.deltaTime);
    }
    public void AddForce(Vector3 input)
    {
        velocity += input;
    }




    //dessa kanske borde flyttas iom att endast spelaren beh�ver dem.. f�r states----------------
    public float groundCheckDistance = 0.05f;
    public bool isGrounded()
    {
        return collisionCaster.CastCollision(transform.position, Vector3.down, groundCheckDistance + skinWidth).collider != null;
    }

    public Vector3 GetVelocity() { return velocity; }
    //------------------------------------------------------------------------------------------

}
