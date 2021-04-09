using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Proximity State", menuName = "New Enemy Proximity State")]
public class EnemyProximityState : State {
    
    private Enemy enemy;

    public float FireCoolDown;
    private float nextFire;

    public GameObject Bullet;

    private bool moving;
    
    protected override void Initialize() {
        enemy = (Enemy) owner;
    }

    public override void Enter() {
        Fire();
        nextFire = Time.time + FireCoolDown;
    }


    public override void RunUpdate() {
        
        //Om denna är false är spelaren inte längre i den yttre sfären
        if(!enemy.ProximityCast(enemy.outerRing)) enemy.stateMachine.ChangeState<EnemyPatrolState>();
        
        //Om denna är true är spelaren innanför den inre sfären
        if(enemy.ProximityCast(enemy.innerRing)) enemy.stateMachine.ChangeState<EnemyBailState>();
        
        enemy.transform.LookAt(enemy.target);

        if (nextFire < Time.time) {
            Fire();
            nextFire = Time.time + FireCoolDown;
        }

        if (!enemy.NavMeshAgent.hasPath)
            enemy.Invoke(() => enemy.SamplePositionOnNavMesh(enemy.transform.position, 10), Random.Range(1, 7));

    }
    
    private void Fire() {
        Instantiate(Bullet, enemy.transform.position + enemy.transform.forward, enemy.transform.rotation);
    }
    
}
