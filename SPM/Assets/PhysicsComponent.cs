using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PhysicsComponent : MonoBehaviour
{
    public LayerMask collisionMask;
    public RaycastHit groundHitInfo;

    private Collider attachedCollider;
    private CollisionCaster collisionCaster;

    [SerializeField] public Vector3 velocity;
    [SerializeField] public float gravity = 10f;
    [SerializeField] protected float skinWidth = 0.05f;

    [Header("Friktion")]
    [Range(0f, 1f)] [SerializeField] float staticFrictionCoefficient = 0.5f;
    [Range(0f, 1f)] [SerializeField] float kineticFrictionCoefficient = 0.35f;
    [Range(0f, 1f)] [SerializeField] float airResistance = 0.35f;

    float gravityMod = 1f;
    public bool AffectedByBlackHoleGravity;
    Vector3 bhGrav = Vector3.zero;
    private void OnEnable()
    {
        attachedCollider = GetComponent<Collider>();

        if (attachedCollider is BoxCollider)
            collisionCaster = new BoxCaster(attachedCollider, collisionMask);

        if (attachedCollider is SphereCollider)
            collisionCaster = new SphereCaster(attachedCollider, collisionMask);

        if (attachedCollider is CapsuleCollider)
            collisionCaster = new CapsuleCaster(attachedCollider, collisionMask);

        if (attachedCollider is MeshCollider)
            collisionCaster = new MeshCaster(attachedCollider, collisionMask);

    }
    
    //float smallNumber = 0.05f;
    float gravityMod = 1f;



    public void Update() {
        Debug.DrawLine(transform.position, transform.position + velocity);
        bhGrav = Vector3.zero;
        AddGravity();
        CheckForCollisions(0);
        transform.position += velocity * Time.deltaTime;
        MoveOutOfGeometry();
    }
    
    private void CheckForCollisions(int i)
    {

        RaycastHit hitInfo = collisionCaster.CastCollision(transform.position, velocity.normalized, velocity.magnitude * Time.deltaTime + skinWidth);

        if (!hitInfo.collider) return;


        RaycastHit normalHitInfo = collisionCaster.CastCollision(transform.position, -hitInfo.normal, hitInfo.distance);
        Vector3 normalForce = General.NormalForce3D(velocity, normalHitInfo.normal);
        if (hitInfo.collider.GetComponent<MovingPlatformV2>())
        {
            velocity += -normalHitInfo.normal * (normalHitInfo.distance - skinWidth);
            velocity += normalForce;
            HandleMovingPlatform(hitInfo, normalForce);
        }
        else
        {
            velocity += -normalHitInfo.normal * (normalHitInfo.distance - skinWidth);
            velocity += normalForce;
            ApplyFriction(normalForce);
        }
        if (i < 10)
            CheckForCollisions(i + 1);
    }

    public float relVelMag = 0f;
    public float treshhold = 0f; 
    private void HandleMovingPlatform(RaycastHit hitInfo, Vector3 normalForce)
    {

        MovingPlatformV2 mp = hitInfo.collider.GetComponent<MovingPlatformV2>();
        Vector3 relativeVelocity = velocity - mp.GetVelocity();
        // relativeVelocity.y = 0;
        

        //debug
        treshhold = normalForce.magnitude * staticFrictionCoefficient;
        relVelMag = relativeVelocity.magnitude;

        Debug.DrawLine(transform.position, transform.position + velocity - relativeVelocity, Color.green); 

        Vector3 nf = General.NormalForce3D(velocity, (velocity - relativeVelocity).normalized);
        if (relativeVelocity.magnitude <= normalForce.magnitude * staticFrictionCoefficient)
        {
            Debug.Log("mag < treshold");
            velocity = mp.GetVelocity();
        }
        else
        {
            relativeVelocity += (General.CalcFriction(General.NormalForce3D(relativeVelocity, mp.GetVelocity() - velocity), relativeVelocity - velocity, staticFrictionCoefficient, kineticFrictionCoefficient));
            velocity -= relativeVelocity * Time.deltaTime;
            //utan apply friction glider man runt på plattformen
            //MED applyfriction så fångar den inte upp plattformens hastighet. FATTAR INTE AOSKNDAOSINDAOSNDAOSDN
        }
    }
    private void MoveOutOfGeometry()
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
            bhGrav = bh.GravitationalPull * (bh.transform.position - transform.position) / Mathf.Pow(Vector3.Distance(bh.transform.position, transform.position), 2) * Time.deltaTime;
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

    private void ApplyFriction(Vector3 normalForce)
    {
        if (velocity.magnitude < normalForce.magnitude * staticFrictionCoefficient)
            velocity = Vector3.zero;
        else
        {
            velocity -= velocity.normalized * normalForce.magnitude * kineticFrictionCoefficient;
        }
        ApplyAirResistance();
        //velocity *= Mathf.Pow(airResistance, Time.deltaTime);
    }
    public void ApplyAirResistance() { velocity *= Mathf.Pow(airResistance, Time.deltaTime); }
    public void AddForce(Vector3 input)
    {
        velocity += input;
    }




    //dessa kanske borde flyttas iom att endast spelaren behöver dem.. för states----------------
    public float groundCheckDistance = 0.05f;
    public bool isGrounded()
    {
        groundHitInfo = collisionCaster.CastCollision(transform.position, Vector3.down, groundCheckDistance + skinWidth);
        return groundHitInfo.collider != null;
    }
    
}
