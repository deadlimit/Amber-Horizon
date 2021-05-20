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
        stateMachine.ChangeState<EnemyDeathState>(); }

    public void Fire() {
        ObjectPooler.Instance.Spawn("Bullet", transform.position + transform.forward + Vector3.up, Quaternion.LookRotation(Target.position - transform.position));
        Pathfinder.agent.isStopped = false;
    }

    public override void ApplyExplosion(GameObject explosionInstance, float blastPower) {
        stateMachine.ChangeState<EnemyExplodedState>();
        base.ApplyExplosion(explosionInstance, blastPower);
    }

    public float GetFireCooldown() { return fireCooldown; }
    public float FleeDistance {get => fleeDistance;}
}
