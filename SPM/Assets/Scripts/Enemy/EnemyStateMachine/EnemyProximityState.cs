using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Proximity State", menuName = "New Enemy Proximity State")]
public class EnemyProximityState : State {
    
    private Forager forager;

    public float FireCoolDown;
    private float nextFire;
    
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
        if(forager.ProximityCast(forager.innerRing)) forager.stateMachine.ChangeState<EnemyTeleportState>();
        
        

        if (nextFire < Time.time) {
            forager.pathfinder.agent.isStopped = true;
            forager.animator.SetTrigger("Shoot");
            nextFire = Time.time + FireCoolDown;
        }
        
        forager.transform.LookAt(forager.target);
    
    }


    
}
