using AbilitySystem;
using EventCallbacks;
using UnityEngine;

public class Bullet : PoolObject {

    [SerializeField] private BulletData bulletData;

    public ForceMode forceMode;
    
    private Rigidbody activeRigidbody;
    
    private void Awake() {
        activeRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        activeRigidbody.AddForce(transform.forward * bulletData.BulletSpeed, forceMode);
    }
    
    private void OnCollisionEnter(Collision other) {
        
        if (((1 << other.gameObject.layer) & bulletData.DamageLayer) != 0)
        {
            EventSystem<PlayerHitEvent>.FireEvent(new PlayerHitEvent(transform, bulletData.Effect));
        }

        ResetBullet();
    }


    
    public override void Initialize(Vector3 position, Quaternion rotation) {
        base.Initialize(position, rotation);
        this.Invoke(ResetBullet, bulletData.ActiveTime);
    }
    
    private void ResetBullet() {
        activeRigidbody.velocity = Vector3.zero;
        activeRigidbody.angularVelocity = Vector3.zero;
        activeRigidbody.ResetCenterOfMass();
        gameObject.SetActive(false);
    }
    
}
