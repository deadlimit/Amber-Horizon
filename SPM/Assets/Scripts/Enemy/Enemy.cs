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
    
    public void Awake() {
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
}
