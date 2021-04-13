using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Proximity State", menuName = "New Enemy Proximity State")]
public class EnemyProximityState : State {
    
    private Forager forager;

    public float FireCoolDown;
    private float nextFire;

    public GameObject Bullet;

    private bool moving;
    
    protected override void Initialize() {
        forager = (Forager) owner;
    }

    public override void Enter() {
        Fire();
        nextFire = Time.time + FireCoolDown;
    }


    public override void RunUpdate() {
        
        //Om denna är false är spelaren inte längre i den yttre sfären
        if(!forager.ProximityCast(forager.outerRing)) forager.stateMachine.ChangeState<EnemyPatrolState>();
        
        //Om denna är true är spelaren innanför den inre sfären
        if(forager.ProximityCast(forager.innerRing)) forager.stateMachine.ChangeState<EnemyBailState>();
        
        forager.transform.LookAt(forager.target);

        if (nextFire < Time.time) {
            Fire();
            nextFire = Time.time + FireCoolDown;
        }

        //if (!forager.NavMeshAgent.hasPath)
           //forager.Invoke(() => forager.SamplePositionOnNavMesh(forager.transform.position, 10), Random.Range(1, 7));

    }
    
    private void Fire() {
        Instantiate(Bullet, forager.transform.position + forager.transform.forward, forager.transform.rotation);
    }
    
}
