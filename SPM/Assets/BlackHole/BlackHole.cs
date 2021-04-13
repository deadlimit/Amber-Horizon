using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public LayerMask physicsLayerMask;
    public LayerMask collisionMask;
    public float GravitationalPull;
    public float gravity = 10f;
    public Vector3 velocity;
    public float airResistance = 0f;
    public float skinWidth;

    public static float BlackHoleRadius;

    public GameObject sphereEffect;
    public float maxRadius;

    public Transform center;
    
    SphereCollider coll;
    public BoxCollider centerColl;

    private bool useGravity = true;
    private float terminalDistance = 0.5f;
    
    public float despawnTimer;
    private float gravitationalPull;
    private Timer deathTimer, timeBeforePull;
    
    private void Awake() {
        gravitationalPull = GravitationalPull;
        GravitationalPull = 0;
        deathTimer = new Timer(despawnTimer);
        timeBeforePull = new Timer(1);
        timeBeforePull.OnTimerReachesZero += TurnOnGravitationPull;
        deathTimer.OnTimerReachesZero += Despawn;
        coll = GetComponent<SphereCollider>();
        BlackHoleRadius = coll.radius;
        centerColl = GetComponent<BoxCollider>();
        Spawn();
    }
    // Update is called once per frame
    void Update() {
        timeBeforePull?.Tick(Time.deltaTime);
        deathTimer.Tick(Time.deltaTime);
        
        GravitationDrag();
        //CheckCenterCollision();
        
        velocity *= Mathf.Pow(airResistance, Time.deltaTime);
        transform.Translate(velocity * Time.deltaTime);
    }
    private void GravitationDrag() {
        bool playerFound = false;
        Collider[] hitcoll = Physics.OverlapSphere(transform.position, coll.radius, physicsLayerMask);
        foreach (Collider c in hitcoll) {

            if (c.tag == "Player")
                playerFound = true;
            
            PhysicsComponent physicsComponent = c.GetComponent<PhysicsComponent>();
            DestructableWall wall = c.GetComponent<DestructableWall>();
            //fysikp�verkan
            if (physicsComponent) {
                
                if (Vector3.Distance(transform.position, c.transform.position) < terminalDistance) {
                    physicsComponent.StopVelocity();
                }
                
                else{
                    IBlackHoleBehaviour blackHoleBehaviour = c.GetComponent<IBlackHoleBehaviour>();
                    IBlackHoleDeath blackHoleDeath = c.GetComponent<IBlackHoleDeath>();
                    if(blackHoleDeath != null)
                        blackHoleDeath.BlackHoleDeath(this);
                    c.GetComponent<PhysicsComponent>().BlackHoleGravity(this, blackHoleBehaviour);
                }
                    
            }else if (wall) {
                IBlackHoleBehaviour blackHoleBehaviour = c.GetComponent<IBlackHoleBehaviour>();
                blackHoleBehaviour.BlackHoleBehaviour(this);
            }

        }

        GameObject.FindGameObjectWithTag("Player").GetComponent<PhysicsComponent>().AffectedByBlackHoleGravity = playerFound;

        if (useGravity)
        {
            //verkar som att overlapbox �r mycket mindre ben�gen att g� igenom v�ggar.
            Collider[] boxHitColl =
            Physics.OverlapBox(transform.position, centerColl.size / 2, Quaternion.identity, collisionMask);
            if (boxHitColl.Length > 0)
            {
                velocity = Vector3.zero;
                useGravity = false;
            }

            velocity += Vector3.down * Time.deltaTime * gravity;
        }    
   

        velocity *= Mathf.Pow(airResistance, Time.deltaTime);
        transform.Translate(velocity * Time.deltaTime);
    }

    
    
    //TODO Den här strök med av någon anledning i en merge och jag hittar verkligen inte orginalet i någon tidigare commit. 
    /*
    private void CheckCenterCollision() {
        if (useGravity) {
            //verkar som att overlapbox är mycket mindre benägen att gå igenom väggar.
            //verkar som att overlapbox �r mycket mindre ben�gen att g� igenom v�ggar.
            Collider[] boxHitColl =
                Physics.OverlapBox(transform.position, centerColl.size / 2, Quaternion.identity, collisionMask);
            if (boxHitColl.Length > 0)
        }
     */

    private void TurnOnGravitationPull() => GravitationalPull = gravitationalPull;

        private void Spawn() {
        StartCoroutine(ExpandRadius(maxRadius));
        StartCoroutine(ExpandSphereEffect(new Vector3(10, 10, 10)));
        gravitationalPull = 25;
    }

    private void Despawn() {
        StartCoroutine(DespawnProcess());
    }

    private IEnumerator DespawnProcess() {
        StartCoroutine(ExpandRadius(0));
        StartCoroutine(ExpandSphereEffect(Vector3.zero));
        
        while (transform.localScale != Vector3.zero) {
            transform.localScale = Vector3.Lerp( transform.localScale, Vector3.zero, Time.deltaTime / 2);
            yield return null;
        }
        
        Destroy(gameObject);
    }
    
    private IEnumerator ExpandRadius(float targetRadius) {
        while (targetRadius < maxRadius) {
            coll.radius = Mathf.Lerp(coll.radius, maxRadius, Time.deltaTime / 2);
            yield return null;
        }

        coll.radius = targetRadius;
    }

    private IEnumerator ExpandSphereEffect(Vector3 targetScale) {
        while (sphereEffect.transform.localScale != targetScale) {
            sphereEffect.transform.localScale = Vector3.Lerp( sphereEffect.transform.localScale, targetScale, Time.deltaTime / 2);
            yield return null;
        }

        sphereEffect.transform.localScale = targetScale;

    }

}
