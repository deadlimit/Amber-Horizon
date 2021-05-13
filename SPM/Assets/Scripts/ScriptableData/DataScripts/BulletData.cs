using AbilitySystem;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet Data", menuName = "ScriptableData/BulletData")]
public class BulletData : ScriptableObject {

    public int BulletSpeed;
    [Header("Time before bullet despawns")]
    public float ActiveTime;
    [Header("Applied effect")]
    public GameplayEffect Effect;
    [Header("Only applies effect on this/these layers")]
    public LayerMask DamageLayer;
}
