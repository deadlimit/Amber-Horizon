using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Proximity State", menuName = "New Enemy Proximity State")]
public class EnemyProximityState : State {
    

    [SerializeField] private float FireCoolDown;
    [SerializeField] private float dropAggroDistance;

    private Forager forager;
    private float nextFire;
    protected override void Initialize() {
        forager = (Forager) owner;
    }

    public override void Enter() {
        forager.Pathfinder.agent.ResetPath();
    }

    public override void RunUpdate() 
    {
       /* if(Vector3.Distance(forager.transform.position, forager.Target.transform.position) > dropAggroDistance)
            stateMachine.ChangeState<EnemyPatrolState>();   */    
        
        //Om denna är true är spelaren innanför den inre sfären
        if(forager.ProximityCast(forager.innerRing)) forager.stateMachine.ChangeState<EnemyTeleportState>();
        
       /* if (Vector3.Distance(forager.transform.position, forager.Target.transform.position) > forager.AttackRange)
        {
            stateMachine.ChangeState<EnemyMoveIntoRangeState>();
        }

        else if (nextFire < Time.time)
        {
            forager.Pathfinder.agent.isStopped = true;
            forager.Animator.SetTrigger("Shoot");
            nextFire = Time.time + FireCoolDown;

        }
        
        forager.transform.LookAt(forager.Target);*/
    
    }


    
}
