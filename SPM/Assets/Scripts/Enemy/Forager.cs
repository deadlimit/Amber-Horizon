using AbilitySystem;
using UnityEngine;

public class Forager : Enemy {

    [Header("Forager Variables")]
    [SerializeField] private float fleeDistance;
    [SerializeField] private float fireCooldown;
    [SerializeField] private float maxRepositionAngle;
    
    [SerializeField] private GameObject Bullet;
    [HideInInspector] public BlackHole activeBlackHole;

    //funderar på att göra range lite olika för varje forager? typ värde mellan 10 och 15 eller något, 
    //så klumpar dom inte ihop sig riktigt på samma sätt

    public bool hitByBlackHole { get; private set; }


    private new void Awake()
    {
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

        //variables are used by the behaviour tree in determining if and how the unit died
        hitByBlackHole = true;
        died = true;

    }

    public void Fire() {
        Transform target = bt.GetBlackBoardValue<Transform>("TargetTransform").GetValue();
        transform.LookAt(target);
        ObjectPooler.Instance.Spawn("Bullet", transform.position + transform.forward + Vector3.up, Quaternion.LookRotation(target.position - transform.position));
    }

    public override void ApplyExplosion(GameObject explosionInstance, float blastPower) {
        //stateMachine.ChangeState<EnemyExplodedState>();
        base.ApplyExplosion(explosionInstance, blastPower);
    }

  
    public float FireCooldown { get=> fireCooldown; }
    public float FleeDistance { get => fleeDistance; }
    public float MaxRepositionAngle { get => maxRepositionAngle; }
}
