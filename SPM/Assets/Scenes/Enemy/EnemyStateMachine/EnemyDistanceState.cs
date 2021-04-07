using UnityEngine;
[CreateAssetMenu(fileName = "Enemy Distance State", menuName = "New Enemy Distance State")]
public class EnemyDistanceState : State {

    private Enemy enemy;
    public float radius;
    protected override void Initialize() {
        enemy = (Enemy) owner;
    }

    public override void RunUpdate() {
        Debug.Log("distance state");
        enemy.physics.HandleInput(Vector3.zero);
        
        if(!enemy.physics.isGrounded())
            enemy.stateMachine.ChangeState<EnemyBailState>();
        
    }
    
}
