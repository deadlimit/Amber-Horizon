using AbilitySystem;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IBlackHoleBehaviour {

    [Header("Enemy variables")]
    [SerializeField] private float attackRange;
    [SerializeField] private float visualRange;
    [SerializeField] private float movementSpeedAttack;
    [SerializeField] private float movementSpeedDefault;
    
    [Header("Patrol")]
    [SerializeField] private float maxWaitTimeOnPatrol;
    [SerializeField] private float minWaitTimeOnPatrol;
    
    [Header("Layermasks")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask lineOfSightMask;

    //TODO; replace names and accesibility of rings
    public float outerRing, innerRing;

    
    public Vector3 originPosition { get; set; }
    public bool died { get; protected set; }

    //Components
    public Rigidbody Rigidbody { get; set; }
    public Animator Animator { get; private set; }
    public AIPathfinder Pathfinder { get; private set; }
    public CapsuleCollider Collider { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public GameplayAbilitySystem AbilitySystem { get; private set; }

    protected BehaviourTree bt;

    public void Awake() {
        bt = GetComponent<BehaviourTree>();
        Animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<CapsuleCollider>();
        Pathfinder = GetComponent<AIPathfinder>();
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
        return Physics.OverlapSphere(transform.position, radius, playerMask).Length > 0;
    }
   
    //"Gives" aggro to nearby enemies
    public bool EnemySeen(float radius)
    {
        Collider [] enemies = Physics.OverlapSphere(transform.position, radius, enemyMask);
        foreach(Collider e in enemies)
        {
            if(e.gameObject.GetComponent<Forager>()?.stateMachine.CurrentState.GetType() == typeof(EnemyProximityState))
            {
                return true;
            }
        }
        return false;
    }
    public void Alert(Transform playerTransform, Transform alerterTransform)
    {
        Debug.Assert(playerTransform);
        bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(playerTransform);
        bt.GetBlackBoardValue<Transform>("AlerterTransform").SetValue(alerterTransform);

        //Cannot call eachother and thereby fuck up the AlerterTransform-value
        //also prevents large chain-pulls if that was ever to be possible due to level design
        bt.GetBlackBoardValue<bool>("HasCalledForHelp").SetValue(true);
    }

    public virtual void BlackHoleBehaviour(BlackHole blackHole) { }

    public virtual void ApplyExplosion(GameObject explosionInstance, float blastPower)
    {
        died = true;
        Vector3 explosionPos = explosionInstance.transform.position;
        float distance = Vector3.Distance(explosionPos, transform.position);
        Vector3 direction = (explosionPos - transform.position).normalized;
        //Move position dependent on distance and direction variables?  

        Animator.SetTrigger("HitByExplosion");
    }

    public virtual void ResetPosition()
    {
        transform.position = originPosition;
        Pathfinder.agent.ResetPath();
    }



    #region GETTERS
    public LayerMask GetPlayerMask() { return playerMask; }
    public LayerMask EnemyMask { get => enemyMask; }
    public LayerMask LineOfSightMask { get => lineOfSightMask; }
    public float AttackRange { get => attackRange; }
    public float VisualRange { get => visualRange; }
    public float MovementSpeedDefault { get => movementSpeedDefault; }
    public float MovementSpeedAttack { get => movementSpeedAttack; }
    public float MaxWaitTimeOnPatrol { get => maxWaitTimeOnPatrol; }
    public float MinWaitTimeOnPatrol { get => minWaitTimeOnPatrol; }
    #endregion
}
