using AbilitySystem;
using UnityEngine;

public class DestructorFist : MonoBehaviour {

    private SphereCollider coll;
    
    private GameplayAbilitySystem abilitySystem;
    
    private void Awake() {
        abilitySystem = GetComponentInParent<GameplayAbilitySystem>();
        coll = GetComponent<SphereCollider>();
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            
            abilitySystem.TryActivateAbilityByTag(GameplayTags.MeleeTag);
            coll.enabled = false;
            enabled = false;
        }
    }
    
}
