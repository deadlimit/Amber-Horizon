
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IBlackHoleDeath {

    [SerializeField] private State[] states;

    public float outerRing, innerRing;
    public LayerMask playerMask;
    public StateMachine stateMachine { get; private set; }
    public Transform target { get; private set; }

    public Animator Animator { get; private set; }
    
    public PhysicsComponent physics { get; private set; }
    

    [HideInInspector] public BlackHole activeBlackHole;
    
    private void Awake() {
        Animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        stateMachine = new StateMachine(this, states);
        physics = GetComponent<PhysicsComponent>();

    }
    private void Update() {

        stateMachine?.RunUpdate();
    }

    /*public void ProximityCast() {

        if (Physics.OverlapSphere(transform.position, outerRing, playerMask).Length > 0)
            if (Physics.OverlapSphere(transform.position, innerRing, playerMask).Length > 0 || !physics.isGrounded()) 
                stateMachine.ChangeState<EnemyBailState>();
            else 
                stateMachine.ChangeState<EnemyProximityState>();
        else 
            stateMachine.ChangeState<EnemyDistanceState>();
        
    }
    */
    public bool ProximityCast(float radius) {
        return Physics.OverlapSphere(transform.position, radius, playerMask).Length > 0;
    }
    
    public void BlackHoleDeath(BlackHole blackHole) {
        if (activeBlackHole) return;
        activeBlackHole = blackHole;
        stateMachine.ChangeState<EnemyDeathState>();
    }
}
