using UnityEngine;

public abstract class CustomPhysics : MonoBehaviour {
    
    public LayerMask CollisionMask;
    public float SkinWidth;

    public enum ForceMode {
        Force,
        Impulse
    }
    
    protected Vector3 velocity;

    [SerializeField] protected float Gravity;
    [SerializeField] protected float Mass;
   
    [SerializeField] protected float MaxGroundVelocity;
    [SerializeField] protected float MaxAirVelocity;
    
    /// <summary>
    /// Applicerar gravitation på objektet över tid (objektets massa * gravitionskoefficienten)
    /// </summary>
    protected void ApplyGravity() {
        velocity += Vector3.down * (Mass * Gravity * Time.deltaTime);
    }

    /// <summary>
    /// Räknar ut normalkraften utifrån given normal.
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="hitNormal"></param>
    /// <returns> En Vector3 med den uträknade normalkaften</returns> 
    public static Vector3 CalculateNormalForce(ref Vector3 direction, Vector3 hitNormal) {
        float dotProduct = Vector3.Dot(direction, hitNormal.normalized);

        if (dotProduct > 0)
            dotProduct = 0;
        
        return -(dotProduct * hitNormal.normalized);
    }
    
    //TODO Avstannar inte tillräckligt när man går på material med låg friktion, bör glida men gör inte det. 
    //3D-versionen
    /// <summary>
    /// Applicerar friktion på objektet. Om objektets kraft understiger den givna normalkraften multiplicerat
    /// med materialets statiska friktion kommer objektet inte att röra sig.
    /// Om kraften är större appliceras den dynamiska friktionen i motsatt riktning. 
    /// </summary>
    /// <param name="normalForceMagnitude"></param>
    /// <param name="currentCollider"></param>
    protected void ApplyFriction(float normalForceMagnitude, Collider currentCollider) {
        if (velocity.magnitude < normalForceMagnitude * currentCollider.material.staticFriction)
            velocity = Vector3.zero;
        else
            velocity -= velocity.normalized * (normalForceMagnitude * currentCollider.material.dynamicFriction);
    }

    //2D-versionen 
    protected void ApplyFriction(float normalForceMagnitude, Collider2D currentCollider) {
        if (velocity.magnitude < normalForceMagnitude * currentCollider.sharedMaterial.friction)
            velocity = Vector3.zero;
        else
            velocity -= velocity.normalized * (normalForceMagnitude * (currentCollider.sharedMaterial.friction * .1f));
    }

    /// <summary>
    /// Fysiskt förflyttar objektet baserat på den nuvarande hastigheten. Förhindrar hastigheten från att överstiga
    /// maxhastigheten. (OBS EJ I Y-LED!).  
    /// </summary>
    protected void ApplyMovement() {
        transform.position += velocity * Time.deltaTime;
    }

    /// <summary>
    /// Adderar inputen till objektets hastighet.
    /// </summary>
    /// <param name="direction"> Önskad riktning (ska vara normaliserad)</param>
    /// <param name="movementOverTime"> Om true sker acceleration över tid, om false appliceras inputen direkt
    /// på hastigheten</param>
    /// <param name="forceMultiplier"></param>
    public void AddForce(Vector3 direction, float forceMultiplier, ForceMode forceMode) {
        switch (forceMode) {
            case ForceMode.Force:
                velocity += direction.normalized * (forceMultiplier * Time.deltaTime);
                break;
            case ForceMode.Impulse:
                 velocity += direction * Mathf.Sqrt(20 * forceMultiplier * Mathf.Abs(Gravity));
                 break;
        }
    }
    
}
