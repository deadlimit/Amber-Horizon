using UnityEngine;

public class BlackHole : PoolObject
{
    public Vector3 velocity { set; get; }
    [Header("LayerMasks")]
    [SerializeField] private LayerMask physicsLayerMask;
    [SerializeField] private LayerMask collisionMask;
       
    [Header("Properties")]
    [SerializeField] private float gravitationalPull;
    [SerializeField] private float effectRadius;
    [SerializeField] private float Lifetime;
    [SerializeField] private GameObject sphereEffect;    
    [SerializeField] private float gravity;
      
    private BoxCollider centerColl;
    private bool useGravity = true;
    private float terminalDistance = 0.5f;    
    private float maxGravitationalPullTemp;  
    private Animator animator;
    
    private void Awake() {
        maxGravitationalPullTemp = gravitationalPull;

        centerColl = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();


    }

    void Update() 
    {       
        GravitationDrag();        
    }
    private void FixedUpdate()
    {
        transform.position += (velocity * Time.fixedDeltaTime);
    }
    private void GravitationDrag() {
        
        Collider[] hitcoll = Physics.OverlapSphere(transform.position, effectRadius, physicsLayerMask);
        
        foreach (Collider collider in hitcoll) {
            Debug.Log(collider.gameObject +"hit");
            PhysicsComponent physicsComponent = collider.GetComponent<PhysicsComponent>();   

            if (physicsComponent) {
                
                if (Vector3.Distance(transform.position, collider.transform.position) < terminalDistance) {
                    physicsComponent.StopVelocity();
                }
                else
                    physicsComponent.BlackHoleGravity(this);
                
            }
            
            IBlackHoleBehaviour blackHoleBehaviour = collider.GetComponent<IBlackHoleBehaviour>();
            if (blackHoleBehaviour != null) 
            {
                blackHoleBehaviour?.BlackHoleBehaviour(this);
            }           
        }
        
        if (useGravity)
        {
            Collider[] boxHitColl =
            Physics.OverlapBox(transform.position, centerColl.size / 2, Quaternion.identity, collisionMask);
            if (boxHitColl.Length > 0)
            {
                DisableGravity();
            }
            else
                ApplyGravity();           
        }
    }
    private void ApplyGravity()
    {
        velocity += Vector3.down * Time.deltaTime * gravity;
    }
    private void DisableGravity()
    {
        velocity = Vector3.zero;
        useGravity = false;
    }
    public float GetGravity()
    {
        return gravity;
    }
    public float GetGravitationalPull()
    {
        return gravitationalPull;
    }


    //Kallas fr�n animation events? B�r f�rtydligas
    private void Die() => gameObject.SetActive(false);
    private void TurnOnGravitationPull() => gravitationalPull = maxGravitationalPullTemp;

    private void StartParticleEffect() {
        GetComponentInChildren<ParticleSystem>().Play();
    }

    public override void Initialize(Vector3 position, Quaternion rotation) {
        base.Initialize(position, rotation);
        useGravity = true;
        Invoke("Die", 2);
        /*animator.SetTrigger("Spawn");        
        
        
        
        this.Invoke(() => {
            animator.SetTrigger("Despawn");
        }, Lifetime);*/
    }
}
