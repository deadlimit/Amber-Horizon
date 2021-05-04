using AbilitySystem;
using UnityEngine;
using EventCallbacks;

[CreateAssetMenu(fileName = "EnemyShoot", menuName = "Abilities/EnemyShoot")]
public class EnemyShoot: GameplayAbility {
    public override void Activate(GameplayAbilitySystem Owner) {
        Enemy parent = Owner.GetComponent<Enemy>();
        Debug.Log("Enemy shoot");
        
        EventSystem<PlayerHitEvent>.FireEvent(new PlayerHitEvent(Owner.transform, this));
        
    }
}
