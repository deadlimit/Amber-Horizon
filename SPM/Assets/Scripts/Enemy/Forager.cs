
using AbilitySystem;
using UnityEngine;

public class Forager : Enemy {

    public GameObject Bullet;
    public float range {get; private set;} = 12f;
    [HideInInspector] public BlackHole activeBlackHole;

    private new void Awake() {
        base.Awake();
    }
    private new void Update()
    {
        base.Update();
        
        stateMachine?.RunUpdate();
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
        GameObject newBullet = Instantiate(Bullet, transform.position + transform.forward + Vector3.up, transform.rotation);
        newBullet.GetComponent<Bullet>().Init(AbilitySystem.GetAbilityByTag(GameplayTags.AttackTag), this);
        Pathfinder.agent.isStopped = false;
    }
}
