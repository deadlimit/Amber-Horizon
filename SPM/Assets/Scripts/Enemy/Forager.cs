using AbilitySystem;
using UnityEngine;

public class Forager : Enemy {

    [SerializeField] private float fleeDistance;
    [SerializeField] private float fireCooldown;
    [SerializeField] private GameObject Bullet;
    [HideInInspector] public BlackHole activeBlackHole;

    //funderar på att göra range lite olika för varje forager? typ värde mellan 10 och 15 eller något, 
    //så klumpar dom inte ihop sig riktigt på samma sätt
    public float range { get; private set; } = 12f;
    public bool hitByBlackHole { get; private set; }
    private BehaviourTree bt;

    private new void Awake()
    {
        bt = GetComponent<BehaviourTree>();
        base.Awake();
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

    public void Fire() {
        Transform target = bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue();
        ObjectPooler.Instance.Spawn("Bullet", transform.position + transform.forward + Vector3.up, Quaternion.LookRotation(target.position - transform.position));
        Pathfinder.agent.isStopped = false;
    }

    public override void ApplyExplosion(GameObject explosionInstance, float blastPower) {
        //stateMachine.ChangeState<EnemyExplodedState>();
        died = true;
        base.ApplyExplosion(explosionInstance, blastPower);
    }

    public void Alert(Transform playerTransform, Transform alerterTransform)
    {
        Debug.Log(gameObject + "Alerted");
        Debug.Assert(playerTransform);
        bt.GetBlackBoardValue<Transform>("TargetTransform").SetValue(playerTransform);
        bt.GetBlackBoardValue<Transform>("AlerterTransform").SetValue(alerterTransform);
        
        //Cannot call eachother and thereby fuck up the AlerterTransform-value
        //also prevents large chain-pulls if that was ever to be possible with level design
        bt.GetBlackBoardValue<bool>("HasCalledForHelp").SetValue(true);
    }
    public float FireCooldown { get=> fireCooldown; }
    public float FleeDistance { get => fleeDistance; }
}
