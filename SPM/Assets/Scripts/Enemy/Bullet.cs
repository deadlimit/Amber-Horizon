using AbilitySystem;
using EventCallbacks;
using UnityEngine;

public class Bullet : PoolObject {

    [SerializeField] private BulletData bulletData;
    
    private Rigidbody activeRigidbody;
    
    private void Awake() {
        activeRigidbody = GetComponent<Rigidbody>();
    }

    private void Update() {
        activeRigidbody.AddForce(transform.forward * bulletData.BulletSpeed);
    }
    
    private void OnCollisionEnter(Collision other) {
        
        if (((1 << other.gameObject.layer) & bulletData.DamageLayer) != 0)
        {
            EventSystem<PlayerHitEvent>.FireEvent(new PlayerHitEvent(transform, bulletData.Effect));
        }
        
        gameObject.SetActive(false);
    }

    public override void Initialize(Vector3 position, Quaternion rotation) {
        base.Initialize(position, rotation);
        
        this.Invoke(() => {
            activeRigidbody.velocity = Vector3.zero;
            activeRigidbody.centerOfMass = Vector3.zero;
            activeRigidbody.angularVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }, bulletData.ActiveTime);
    }
    
    
    
}
