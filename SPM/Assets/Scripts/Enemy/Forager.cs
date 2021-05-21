using AbilitySystem;
using UnityEngine;

public class Forager : Enemy {

    [SerializeField] private float fleeDistance;
    [SerializeField] private float fireCooldown;
    [SerializeField] private GameObject Bullet;
    [HideInInspector] public BlackHole activeBlackHole;


    //Referens till BT kanske eg. ska ligga i Enemy, har inte kommit till destructor än så jag gör det här så länge
    private BehaviourTree bt;
    //funderar på att göra range lite olika för varje forager? typ värde mellan 10 och 15 eller något, 
    //så klumpar dom inte ihop sig riktigt på samma sätt
    public float range { get; private set; } = 12f;
    public bool hitByBlackHole { get; private set; }

    private new void Awake()
    {
        base.Awake();
        bt = GetComponent<BehaviourTree>(); 
    }
    private new void Update()
    {
        base.Update();
        //stateMachine?.RunUpdate();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        if (Pathfinder == null || !Pathfinder.agent.hasPath) return;
        Gizmos.DrawLine(transform.position, Pathfinder.agent.destination);
        Gizmos.DrawWireSphere(Pathfinder.agent.destination, .5f);

    }
    public override void BlackHoleBehaviour(BlackHole blackHole) {
        if (activeBlackHole) return;
        activeBlackHole = blackHole;

        //TODO; kommentera det här
        hitByBlackHole = true;
        died = true;

        //stateMachine.ChangeState<EnemyDeathState>(); 
    }

    public void Fire(Transform target) {
        ObjectPooler.Instance.Spawn("Bullet", transform.position + transform.forward + Vector3.up, Quaternion.LookRotation(target.position - transform.position));
        Pathfinder.agent.isStopped = false;
    }

    public override void ApplyExplosion(GameObject explosionInstance, float blastPower) {
        //stateMachine.ChangeState<EnemyExplodedState>();
        died = true;
        base.ApplyExplosion(explosionInstance, blastPower);
    }

    public float GetFireCooldown() { return fireCooldown; }
    public float FleeDistance {get => fleeDistance;}
}
