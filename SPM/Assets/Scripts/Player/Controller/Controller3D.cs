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
             //   CameraFocusEvent cameraFocusEvent = new CameraFocusEvent();
                //cameraFocusEvent.newFocusTarget = GameObject.FindGameObjectWithTag("KeyCameraTarget").transform;
               // EventSystem<CameraFocusEvent>.FireEvent(cameraFocusEvent);
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
    
    
}


