
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IBlackHoleDeath {

    [SerializeField] private State[] states;

    public float outerRing, innerRing;
    
    public LayerMask playerMask;
    public StateMachine stateMachine { get; private set; }
    public Transform target { get; private set; }

    public bool notMoving;
    
    public PhysicsComponent physics { get; private set; }
    public GameObject Bullet;

    public bool processOfDying;
    
    [HideInInspector] public NavMeshAgent MeshAgent { get; private set; }

    [HideInInspector] public BlackHole activeBlackHole;
    
    private void Awake() {
        
        target = GameObject.FindGameObjectWithTag("Player").transform;
        MeshAgent = GetComponent<NavMeshAgent>();
        stateMachine = new StateMachine(this, states);

        notMoving = true;
        physics = GetComponent<PhysicsComponent>();
        processOfDying = false;

    }
    private void Update() {
        
        if (notMoving && !processOfDying)
            ProximityCast();

        if(!physics.isGrounded() && notMoving && !processOfDying)
            stateMachine.ChangeState<EnemyBailState>();
        
        stateMachine?.RunUpdate();
    }

    public void ProximityCast() {

        if (Physics.OverlapSphere(transform.position, outerRing, playerMask).Length > 0)
            if (Physics.OverlapSphere(transform.position, innerRing, playerMask).Length > 0 || !physics.isGrounded()) 
                stateMachine.ChangeState<EnemyBailState>();
            else 
                stateMachine.ChangeState<EnemyProximityState>();
        else 
            stateMachine.ChangeState<EnemyDistanceState>();
        
    }

    public void Fire(int shots) {
        for(int i = 0; i < shots; i++)
         Instantiate(Bullet, transform.position + transform.forward, transform.rotation);
    }
    
    public void BlackHoleDeath(BlackHole blackHole) {
        print("blackhole");
        if (processOfDying) return;
        processOfDying = true;
        activeBlackHole = blackHole;
        stateMachine.ChangeState<EnemyDeathState>();
    }
}
