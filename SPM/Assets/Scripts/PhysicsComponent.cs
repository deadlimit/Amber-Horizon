using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PhysicsComponent : MonoBehaviour
{
    [SerializeField] private LayerMask collisionMask;
    
    [Header("Values")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float gravity = 10f;
    [SerializeField] private float skinWidth = 0.05f;
    [SerializeField] private float inputThreshold = 0.1f;
    [SerializeField] private float gravityModifier = 1f;

    [Header("Friction")]
    [Range(0f, 1f)] [SerializeField] private float staticFrictionCoefficient = 0.5f;
    [Range(0f, 1f)] [SerializeField] private float kineticFrictionCoefficient = 0.35f;
    [Range(0f, 1f)] [SerializeField] private float airResistance = 0.35f;
    
    //Collision
    private Collider attachedCollider;
    private CollisionCaster collisionCaster;

    public Vector3 velocity;
    public RaycastHit groundHitInfo { get; private set; }
    private Vector3 bhGrav = Vector3.zero;
    private void OnEnable()
    {
        attachedCollider = GetComponent<Collider>();
        
        if(GetComponent<PlayerController>())
            maxSpeed = GetComponent<PlayerController>().GetMaxSpeed();

        if (attachedCollider is BoxCollider)
            collisionCaster = new BoxCaster(attachedCollider, collisionMask);

        if (attachedCollider is SphereCollider)
            collisionCaster = new SphereCaster(attachedCollider, collisionMask);

        if (attachedCollider is CapsuleCollider)
            collisionCaster = new CapsuleCaster(attachedCollider, collisionMask);

        if (attachedCollider is MeshCollider)
            collisionCaster = new MeshCaster(attachedCollider, collisionMask);

    }
    
    public void Update()
    {
        bhGrav = Vector3.zero;
        AddGravity();
        CheckForCollisions(0);
        ClampSpeed();

        //Silvertejpslösning för att inte få -Infinity eller NaN
        if (float.IsNaN(velocity.x) == false && float.IsNegativeInfinity(velocity.x) == false && float.IsPositiveInfinity(velocity.x) == false  )
            transform.position += velocity * Time.deltaTime;
        
        MoveOutOfGeometry();
    }
    
    public Vector3 GetXZMovement()
    {
        return new Vector3(velocity.x, 0, velocity.z);
    }
    private void ClampSpeed()
    {
        float temp = velocity.y;
        velocity = maxSpeed != 0 ? Vector3.ClampMagnitude(new Vector3(velocity.x, 0, velocity.z), maxSpeed) : velocity;
        velocity.y = temp; 
    }
    private void CheckForCollisions(int i)
    {
        RaycastHit hitInfo = collisionCaster.CastCollision(transform.position, velocity.normalized, velocity.magnitude * Time.deltaTime + skinWidth);
        if (hitInfo.collider && hitInfo.collider.isTrigger == false)
        {

            RaycastHit normalHitInfo = collisionCaster.CastCollision(transform.position, -hitInfo.normal, hitInfo.distance);
            Vector3 normalForce = PhysicsFunctions.NormalForce3D(velocity, normalHitInfo.normal);

            velocity += -normalHitInfo.normal * (normalHitInfo.distance - skinWidth);
            velocity += normalForce;

            if (hitInfo.collider.TryGetComponent<MovingPlatform>(out var platform))
            {
                HandleMovingPlatform(platform, hitInfo, normalForce);
            }
            else
            {
                ApplyFriction(normalForce);
            }
            if (i < 10)
                CheckForCollisions(i + 1);
        }
    }

    private void HandleMovingPlatform(MovingPlatform mp, RaycastHit hitInfo, Vector3 normalForce)
    {
        Vector3 platformVelocity = mp.GetVelocity();
        Vector3 relativeVelocity = velocity - platformVelocity;        

        if (relativeVelocity.magnitude <= normalForce.magnitude * staticFrictionCoefficient)
        {
            velocity = platformVelocity;
        }
        else
        {
            velocity += platformVelocity  * Time.deltaTime;
            velocity -=  relativeVelocity.normalized * normalForce.magnitude * kineticFrictionCoefficient;           
            ApplyAirResistance();
        }
    }
    private void MoveOutOfGeometry()
    {
        Collider[] colliders = collisionCaster.OverlapCast(transform.position);

        if (colliders.Length < 1) return;

        foreach (Collider currentCollider in colliders)
        {
            if (currentCollider == attachedCollider || currentCollider.isTrigger)
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
            velocity += PhysicsFunctions.NormalForce3D(velocity, seperationVector);
        }

    }
    public void BlackHoleGravity(BlackHole bh) {
        if (gravity == 0)
            return;

        bhGrav = bh.GetGravitationalPull() * (bh.transform.position - transform.position) / Mathf.Pow(Vector3.Distance(bh.transform.position, transform.position), 2) * Time.fixedDeltaTime;
        velocity += bhGrav;
        ApplyFriction(PhysicsFunctions.NormalForce3D(velocity, bh.transform.position - transform.position));
        bhGrav = Vector3.zero;      
    }
    public void StopVelocity()
    {
        velocity -= velocity * 0.02f;

        //gravityMod assignment m�ste just nu appliceras kontinuerligt, oavsett hur man vill g�ra med velocity
        gravityModifier = 0.1f;
    }
    protected void AddGravity()
    {
        Vector3 gravityMovement = gravity * Vector3.down * Time.deltaTime * gravityModifier;

        if (bhGrav != Vector3.zero)
        {
            gravityMovement = bhGrav;
            velocity *= 0.05f;
        }

        velocity += gravityMovement;
        gravityModifier = 1f;
    }
    public void ApplyFriction(Vector3 normalForce)
    {
        if (velocity.magnitude < normalForce.magnitude * staticFrictionCoefficient)
            velocity = Vector3.zero;
        else
        {
            velocity -= velocity.normalized * normalForce.magnitude * kineticFrictionCoefficient;
        }
        ApplyAirResistance();
    }
    public void ApplyAirResistance() { velocity *= Mathf.Pow(airResistance, Time.deltaTime); }
    public void AddForce(Vector3 input)
    {
        velocity += input.magnitude < inputThreshold? Vector3.zero : input * Time.deltaTime;

    }

    
}
