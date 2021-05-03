using AbilitySystem;
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
    public LayerMask EnemyMask;

    public GameplayAbilitySystem AbilitySystem { get; private set; }
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

    private void Start() {
        AbilitySystem = GetComponent<GameplayAbilitySystem>();
    }

    public void Update() {
        Animator.SetFloat("DistanceToTarget", Pathfinder.agent.velocity.magnitude);
    }
    
    public bool ProximityCast(float radius) {
        return Physics.OverlapSphere(transform.position, radius, PlayerMask).Length > 0;
    }
    public bool EnemySeen(float radius)
    {
        Collider [] enemies = Physics.OverlapSphere(transform.position, radius, EnemyMask);
        foreach(Collider e in enemies)
        {
            if(e.gameObject.GetComponent<Enemy>().stateMachine.currentState.GetType() == typeof(EnemyProximityState))
            {
                Debug.Log("EnemySeen Success");
                return true;
            }
        }
        return false;
    }
    
    
    public virtual void BlackHoleBehaviour(BlackHole blackHole) { Debug.Log("hello");}

    public void ApplyExplosion(GameObject explosionInstance, float blastPower)
    {
        Vector3 explosionPos = explosionInstance.transform.position;
        float distance = Vector3.Distance(explosionPos, transform.position);
        Vector3 direction = (explosionPos - transform.position).normalized;
        
        Animator.SetTrigger("HitByExplosion");
        Pathfinder.agent.enabled = false;
        stateMachine.ChangeState<DestructorDeathState>();
   
    }
}
