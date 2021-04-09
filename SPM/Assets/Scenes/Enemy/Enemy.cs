using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IBlackHoleDeath {

    [SerializeField] private State[] states;

    public Transform patrolAreaCenter;
    public float patrolAreaRadius;
    public float patrolRangeFromCenter;
    public float outerRing, innerRing;
    public LayerMask playerMask;
    public float MovementSpeed;
    public StateMachine stateMachine { get; private set; }
    public Transform target { get; private set; }

    public Animator Animator { get; private set; }
    public NavMeshAgent NavMeshAgent { get; private set; }
    public PhysicsComponent physics { get; private set; }
    
    [HideInInspector] public BlackHole activeBlackHole;
    
    private void Awake() {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        stateMachine = new StateMachine(this, states);
        physics = GetComponent<PhysicsComponent>();
    }
    
    private void Update() {

        stateMachine?.RunUpdate();
    }

    public bool ProximityCast(float radius) {
        return Physics.OverlapSphere(transform.position, radius, playerMask).Length > 0;
    }

    public void SamplePositionOnNavMesh(Vector3 origin, float originRadius) {
        Vector3 randomPositionInsidePatrolArea = Random.insideUnitSphere * originRadius + origin;
        NavMesh.SamplePosition(randomPositionInsidePatrolArea, out var hitInfo, 10, NavMesh.AllAreas);

        NavMeshAgent.SetDestination(hitInfo.position);
    }

    public void BlackHoleDeath(BlackHole blackHole) {
        if (activeBlackHole) return;
        activeBlackHole = blackHole;
        stateMachine.ChangeState<EnemyDeathState>();
    }
    
}
