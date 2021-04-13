using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Proximity State", menuName = "New Enemy Proximity State")]
public class EnemyProximityState : State {
    
    private Forager forager;

    public float FireCoolDown;
    private float nextFire;

    public GameObject Bullet;
    
    protected override void Initialize() {
        forager = (Forager) owner;
    }

    public override void Enter() {
        forager.pathfinder.agent.ResetPath();
    }

    public override void RunUpdate() {
        
        //Om denna är false är spelaren inte längre i den yttre sfären
        if(!forager.ProximityCast(forager.outerRing)) forager.stateMachine.ChangeState<EnemyPatrolState>();
        
        //Om denna är true är spelaren innanför den inre sfären
        if(forager.ProximityCast(forager.innerRing)) forager.stateMachine.ChangeState<EnemyBailState>();
        
        forager.transform.LookAt(forager.target);

        if (nextFire < Time.time) {
            Instantiate(Bullet, forager.transform.position + forager.transform.forward + Vector3.up, forager.transform.rotation);
            nextFire = Time.time + FireCoolDown;
        }
        
        if (forager.pathfinder.agent.remainingDistance < 1f)
            forager.Invoke(() => forager.pathfinder.SamplePositionOnNavMesh(forager.transform.position, 10));
    
    }
    
}
