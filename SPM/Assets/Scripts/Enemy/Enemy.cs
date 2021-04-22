using UnityEngine;

public abstract class Enemy : MonoBehaviour, IBlackHoleBehaviour {

    public float outerRing, innerRing;
    public Rigidbody Rigidbody { get; set; }
    public Animator Animator { get; private set; }
    public AIPathfinder Pathfinder { get; private set; }
    public Transform Target { get; private set; }
    public CapsuleCollider Collider { get; private set; }
    public Vector3 originPosition { get; set; }
    
    public LayerMask PlayerMask;
    
    [SerializeField] private State[] states;
    
    public StateMachine stateMachine { get; private set; }

    private PhysicsComponent physics;
    public void Awake() {
        physics = GetComponent<PhysicsComponent>();
        stateMachine = new StateMachine(this, states);
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<CapsuleCollider>();
        Pathfinder = GetComponent<AIPathfinder>();
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        originPosition = transform.position;
    }

    public void Update() {
        Animator.SetFloat("DistanceToTarget", Pathfinder.agent.velocity.magnitude);
    }
    
    public bool ProximityCast(float radius) {
        return Physics.OverlapSphere(transform.position, radius, PlayerMask).Length > 0;
    }
    
    
    public virtual void BlackHoleBehaviour(BlackHole blackHole) { Debug.Log("hello");}

    public void ApplyExplosion(GameObject explosionInstance)
    {
        Debug.Log("applyExplosion");
        Vector3 explosionPos = explosionInstance.transform.position;
        float distance = Vector3.Distance(explosionPos, transform.position);
        Vector3 direction = (explosionPos - transform.position).normalized;
        

         /*physics.StopVelocity();
         physics.AddForce(-direction * (500 / distance) + 100 * Vector3.up);
         Navmesh skriver över? 
         stäng av navmesh, aktivera animation
          */
        
        stateMachine.ChangeState<DestructorDeathState>();
   
    }
}
