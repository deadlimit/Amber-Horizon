using AbilitySystem;
using EventCallbacks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controller3D : MonoBehaviour
{
    private Camera activeCamera;
    public Vector3 velocity;
    [SerializeField] float acceleration = 4f;
    [SerializeField]float maxSpeed;

    public float jumpHeight = 4f;
    public Vector3 input = Vector3.zero;
    public PhysicsComponent playerPhys;
    public LauncherBlackHole lbh;
    public float deceleration = 1f;
    
    private GameplayAbilitySystem abilitySystem;
    
    [Header("StateMachine")]
    public State[] states;
    private StateMachine stateMachine;
    private bool jump;

    private Animator animator;
    
    /*obsolete, ska ta bort det så fort jag är säker på min sak-------
    public BlackHole blackHole;
    public float launchSpeedXZ = 5f;
    public bool debugPath; 
    -----------------------------------------------------------*/

    void Awake() {
        activeCamera = Camera.main;
        playerPhys = GetComponent<PhysicsComponent>();
        stateMachine = new StateMachine(this, states);
        animator = GetComponent<Animator>();
    }
    
    private void Start() {
        abilitySystem = GetComponent<GameplayAbilitySystem>();
    }

    public void SetInput(Vector3 inp)
    {
        input = inp;
        
    }

    private void RotateTowardsCameraDirection() {

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, activeCamera.transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
    void PlayerDirection() {     
        input = activeCamera.transform.rotation * input;
        RotateTowardsCameraDirection();
        Vector3 angle = Vector3.ProjectOnPlane(input, playerPhys.groundHitInfo.normal).normalized;
        input = angle * input.magnitude;
        if (input.magnitude > float.Epsilon)
            Accelerate();
        else
            Decelerate();
        if (jump)
        {
            velocity.y += jumpHeight;
            jump = false;
        }
    }
    private void Accelerate()
    {
            velocity = input * acceleration * Time.deltaTime;
    }
    private void Decelerate() 
    {
        float temp = velocity.y;
        velocity = (-deceleration  * playerPhys.velocity * Time.deltaTime);
        velocity.y = temp;
    }


    void Update() {
        
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            Time.timeScale = Time.timeScale == .3f ? Time.timeScale = 1 : Time.timeScale = .3f;
        }
        
        if (Input.GetKeyDown(KeyCode.E))
            abilitySystem.TryActivateAbilityByTag(GameplayTags.MovementAbilityTag);


        if (Input.GetKeyDown(KeyCode.I)) {
            bool zoomIn = !animator.GetBool("ShowKey");


            if (!zoomIn) {
                CameraEvent cameraEvent = new CameraEvent();
                cameraEvent.newTarget = GameObject.FindGameObjectWithTag("KeyCameraTarget").transform;
                EventSystem<CameraEvent>.FireEvent(cameraEvent);
            }

            animator.SetBool("ShowKey", zoomIn);

        }
            
        
        if (Input.GetMouseButtonDown(1))
            lbh.Activate();
        if (Input.GetMouseButtonUp(1))
            lbh.Deactivate();
    }
    public void SetJump()
    {
        jump = true;
    }

    
    public PhysicsComponent GetPhysics() { return playerPhys; }
    
    
    
    //TODO Första gången spelaren fastnar i ett svarthål är första dashen mycket längre än följande dasher, vet inte varför
    /// <summary>
    /// Dash. Ska kunna dasha åt input-hållet? Dashar endast rakt fram just nu.
    /// 
    /// </summary>
    /// <returns></returns>
    [CanBeNull]
    private IEnumerator Dash() {

        /* effects.SetTrigger("Dash");
         //Spara gravitationen innan man sätter den till 0
         float gravity = playerPhys.gravity;

         Vector3 cameraForwardDirection = activeCamera.transform.forward;

         //Nollar y-axeln för att bara dasha framåt.
         cameraForwardDirection.y = 0;

         //Stänger av gravitationen och nollställer hastigheten för att endast dash-velociteten ska gälla. 
         Vector3 forwardMomentum = new Vector3(playerPhys.velocity.x, 0f, playerPhys.velocity.z);
         playerPhys.velocity = Vector3.zero;
         playerPhys.gravity = 0;
         //playerPhys.bhGrav = Vector3.zero;


         velocity = playerPhys.AffectedByBlackHoleGravity ? cameraForwardDirection * (BlackHole.BlackHoleRadius * blackHoleGravityDashForce) : cameraForwardDirection * dashLength;
         playerPhys.AddForce(velocity);

         Debug.DrawLine(transform.position, velocity, Color.red);

         //Vänta .4 sekunder innan man sätter på gravitationen igen. 
         yield return new WaitForSeconds(timeWithoutGravity);

         playerPhys.velocity = transform.forward * 2;
         playerPhys.gravity = gravity;

         playerPhys.AffectedByBlackHoleGravity = false;
         playerPhys.velocity = forwardMomentum;*/
        yield return null;
    }
}


