using UnityEngine;

public class Destructor : Enemy {
    
    private void Start() {
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
