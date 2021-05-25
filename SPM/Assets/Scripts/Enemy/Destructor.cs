using UnityEngine;

public class Destructor : Enemy {
    
    protected new void Start() {
        base.Start();
        stateMachine.ChangeState<DestructorPatrolState>();
    }
    public override void ResetPosition()
    {
        stateMachine.ChangeState<DestructorResetState>();
        base.ResetPosition();
    }
    private new void Update() {
        base.Update();
        
       // stateMachine.RunUpdate();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (Pathfinder == null || !Pathfinder.agent.hasPath) return;
        Gizmos.DrawLine(transform.position, Pathfinder.agent.destination);
        Gizmos.DrawWireSphere(Pathfinder.agent.destination, .5f);

    }
    public override void ApplyExplosion(GameObject explosionInstance, float blastPower)
    {
        stateMachine.ChangeState<DestructorDeathState>();
        base.ApplyExplosion(explosionInstance, blastPower);
    }
    
}
