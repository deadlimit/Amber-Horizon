using AbilitySystem;
using UnityEngine;

public class DestructorFist : MonoBehaviour {
    
    private SphereCollider coll;   
    private GameplayAbilitySystem abilitySystem;
    
    private void Awake() {
        enabled = false;
        coll = GetComponent<SphereCollider>();
    }

    private void Start() {
        abilitySystem = GetComponentInParent<GameplayAbilitySystem>();
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) 
        {            
            abilitySystem.TryActivateAbilityByTag(GameplayTags.MeleeTag);
            coll.enabled = false;
            enabled = false;
        }
    }
    
}
