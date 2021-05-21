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
    public bool died { get; protected set; }

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
    private void OnDisable()
    {
        PlayerReviveListener.enemyList.Remove(this);
    }
    protected void Start() {
        PlayerReviveListener.enemyList.Add(this); 
        AbilitySystem = GetComponent<GameplayAbilitySystem>();
    }

    public void Update() {
        Animator.SetFloat("DistanceToTarget", Pathfinder.agent.velocity.magnitude);
    }
    
    public bool ProximityCast(float radius) {
        return Physics.OverlapSphere(transform.position, radius, PlayerMask).Length > 0;
    }
   
    //Låter foragers "ge" andra foragers & destructors aggro, men 
    //destructors har ju inte proximityState så de kan inte göra det
    public bool EnemySeen(float radius)
    {
        Collider [] enemies = Physics.OverlapSphere(transform.position, radius, EnemyMask);
        foreach(Collider e in enemies)
        {
            if(e.gameObject.GetComponent<Forager>()?.stateMachine.CurrentState.GetType() == typeof(EnemyProximityState))
            {
                return true;
            }
        }
        return false;
    }
    
    
    public virtual void BlackHoleBehaviour(BlackHole blackHole) { }

    public virtual void ApplyExplosion(GameObject explosionInstance, float blastPower)
    {
        Vector3 explosionPos = explosionInstance.transform.position;
        float distance = Vector3.Distance(explosionPos, transform.position);
        Vector3 direction = (explosionPos - transform.position).normalized;
        //Flytta position beroende på distance och direction? 

        Animator.SetTrigger("HitByExplosion");
    }

    //Destructor must also change state to not instantly set destination towards player respawn, hence virtual
    public virtual void ResetPosition()
    {
        transform.position = originPosition;
        Pathfinder.agent.ResetPath();
    }

    public LayerMask GetPlayerMask() { return PlayerMask; }
}
