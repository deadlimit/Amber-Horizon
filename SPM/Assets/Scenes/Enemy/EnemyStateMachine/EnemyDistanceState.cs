using UnityEngine;
[CreateAssetMenu(fileName = "Enemy Distance State", menuName = "New Enemy Distance State")]
public class EnemyDistanceState : State {

    private Enemy enemy;

    protected override void Initialize() {
        enemy = (Enemy) owner;
    }

    public override void RunUpdate() {
        enemy.transform.LookAt(enemy.target);
        if(!enemy.physics.isGrounded())
            enemy.stateMachine.ChangeState<EnemyBailState>();
        
    }
    
}
