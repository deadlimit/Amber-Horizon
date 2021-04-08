using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Death State", menuName = "New Enemy Death State")]
public class EnemyDeathState : State {

    private Enemy enemy;
    
    protected override void Initialize() {
        enemy = (Enemy) owner;
    }

    
    public override void RunUpdate() {
        if (enemy.activeBlackHole != null) {
            enemy.transform.LookAt(enemy.activeBlackHole.transform);

            enemy.transform.position = Vector3.Lerp(enemy.transform.position, enemy.activeBlackHole.center.transform.position, Time.deltaTime);

            enemy.transform.localScale = Vector3.Lerp(enemy.transform.localScale, Vector3.zero, Time.deltaTime);
        }
        
        Destroy(enemy.gameObject, 3);
            
    }      
    
}
