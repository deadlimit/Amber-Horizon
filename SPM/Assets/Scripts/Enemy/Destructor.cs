using UnityEngine;

public class Destructor : Enemy {

    public TextMesh text;
    
    private void Start() {
        stateMachine.ChangeState<DestructorPatrolState>();
    }
    
    private new void Update() {
        base.Update();
        
        stateMachine.RunUpdate();
        text.text = stateMachine.currentState.ToString();

    }

    public override void ApplyExplosion(GameObject explosionInstance, float blastPower)
    {
        stateMachine.ChangeState<DestructorDeathState>();
        base.ApplyExplosion(explosionInstance, blastPower);
    }
    
}
