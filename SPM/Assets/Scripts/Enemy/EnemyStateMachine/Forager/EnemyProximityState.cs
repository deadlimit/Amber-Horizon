using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Proximity State", menuName = "New Enemy Proximity State")]
public class EnemyProximityState : State {
    
    private Forager forager;

    public float FireCoolDown;
    private float nextFire;
    public float dropAggroDistance;
    private int frame;
    protected override void Initialize() {
        forager = (Forager) owner;
    }

    public override void Enter() {
        Debug.Log("Prox State");
        forager.Pathfinder.agent.ResetPath();
    }

    public override void RunUpdate() {


        if(Vector3.Distance(forager.transform.position, forager.Target.transform.position) > dropAggroDistance)
            stateMachine.ChangeState<EnemyPatrolState>();
        
        //Om denna är false är spelaren inte längre i den yttre sfären
        //if(!forager.ProximityCast(forager.outerRing)) forager.stateMachine.ChangeState<EnemyPatrolState>();

        
        //Om denna är true är spelaren innanför den inre sfären
        if(forager.ProximityCast(forager.innerRing)) forager.stateMachine.ChangeState<EnemyTeleportState>();

        // för att det här ska funka måste vi göra ett nytt state,
        //typ MovingIntoRangeState, eller göra att vi förbipasserar resten av logiken när vi har uppdaterat SetDestination..
        
        if (Vector3.Distance(forager.transform.position, forager.Target.transform.position) > forager.range)
        {
            stateMachine.ChangeState<EnemyMoveIntoRangeState>();
        }

        else if (nextFire < Time.time)
        {
            forager.Pathfinder.agent.isStopped = true;
            forager.Animator.SetTrigger("Shoot");
            nextFire = Time.time + FireCoolDown;

        }
        
        forager.transform.LookAt(forager.Target);
    
    }


    
}
