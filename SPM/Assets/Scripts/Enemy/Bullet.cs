using AbilitySystem;
using EventCallbacks;
using UnityEngine;

public class Bullet : PoolObject {
    
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameplayEffect effect;
    [SerializeField] private LayerMask hitLayer;
    private Rigidbody activeRigidbody;
    
    private void Awake() {
        activeRigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        activeRigidbody.AddForce(transform.forward * bulletSpeed);
    }
    
    private void OnTriggerEnter(Collider other) {
        
        if (((1 << other.gameObject.layer) & hitLayer) != 0)
        {
            GameplayAbilitySystem playerAbilitySystem = other.gameObject.GetComponent<GameplayAbilitySystem>();
            playerAbilitySystem.ApplyEffectToSelf(effect);
            EventSystem<PlayerHitEvent>.FireEvent(new PlayerHitEvent(transform, effect));
        }
        
        gameObject.SetActive(false);
    }
    
}
