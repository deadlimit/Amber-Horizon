using UnityEngine;

public class Destructor : Enemy {
    
    protected new void Start() {
        base.Start();
        stateMachine.ChangeState<DestructorPatrolState>();
    }
    private new void Update() {
        base.Update();
        
        stateMachine.RunUpdate();

    }

    public override void ApplyExplosion(GameObject explosionInstance, float blastPower)
    {
        stateMachine.ChangeState<DestructorDeathState>();
        base.ApplyExplosion(explosionInstance, blastPower);
    }
    
}
