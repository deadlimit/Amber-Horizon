using UnityEngine;


public abstract class Enemy : MonoBehaviour, IBlackHoleBehaviour {

    public float outerRing, innerRing;
    public PhysicsComponent PhysicsComponent { get; set; }
    public Animator Animator { get; private set; }
    public AIPathfinder Pathfinder { get; private set; }
    public Transform Target { get; private set; }
    public CapsuleCollider Collider { get; private set; }
    
    public LayerMask PlayerMask;
    
    [SerializeField] private State[] states;
    
    public StateMachine stateMachine { get; private set; }
    
    public void Awake() {
        stateMachine = new StateMachine(this, states);
        Animator = GetComponent<Animator>();
        PhysicsComponent = GetComponent<PhysicsComponent>();
        Collider = GetComponent<CapsuleCollider>();
        Pathfinder = GetComponent<AIPathfinder>();
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Update() {
        Animator.SetFloat("DistanceToTarget", Vector3.Distance(transform.position, Pathfinder.agent.destination));
    }
    
    public bool ProximityCast(float radius) {
        return Physics.OverlapSphere(transform.position, radius, PlayerMask).Length > 0;
    }
    
    
    public virtual void BlackHoleBehaviour(BlackHole blackHole) { Debug.Log("hello");}
}
