using AbilitySystem;
using UnityEngine;

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
    
    [SerializeField] private int keyFragments = 0;

    private GameplayAbilitySystem abilitySystem;
    
    [Header("StateMachine")]
    public State[] states;
    private StateMachine stateMachine;
    private bool jump;

    /*obsolete, ska ta bort det så fort jag är säker på min sak-------
    public BlackHole blackHole;
    public float launchSpeedXZ = 5f;
    public bool debugPath; 
    -----------------------------------------------------------*/

    void Awake() {
        activeCamera = Camera.main;
        playerPhys = GetComponent<PhysicsComponent>();
        stateMachine = new StateMachine(this, states);
        
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
        Accelerate();
    }
    private void Accelerate()
    {
        velocity = input * acceleration * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
    
        //det här är skräp
        if (jump) 
        {
            velocity.y += jumpHeight;
            jump = false;
        }
    }

    void Update() {

        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            Time.timeScale = Time.timeScale == .3f ? Time.timeScale = 1 : Time.timeScale = .3f;
        }
        
        stateMachine.RunUpdate();        
        PlayerDirection();     
        playerPhys.AddForce(velocity);
        
        if (Input.GetKeyDown(KeyCode.E))
            abilitySystem.TryActivateAbilityByTag(GameplayTags.MovementAbilityTag);
            
            if (Input.GetKeyDown(KeyCode.B)) {
            DashAbility dashAbility = AbilityLoader.CreateAbility<DashAbility, MovementTag>();
            dashAbility.dashLength = 20;
            dashAbility.timeWithOutGravity = .4f;
            abilitySystem.GrantAbility(dashAbility);
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
    
    
    public void AddKeyFragment()
    {
        keyFragments += 1;
    }

    //obsolete men inte redo att radera allt riktigt än
   /* private void LaunchBH() 
    {
        BlackHole bh = Instantiate(blackHole, transform.position, Quaternion.identity);
        bh.velocity = transform.TransformDirection(BHTrajectory() * launchSpeedXZ);
    }

    public float launchSpeedY = 10f;
    private Vector3 BHTrajectory() 
    {
        return (cam.transform.forward + Vector3.up * launchSpeedY).normalized;
        //man vill nog lägga till spelarens velocitet efter uträkningen här
    }*/

   /* void DrawPath() 
    {
        Debug.Log("Drawpath: bh är " + bh);
        float timeToTarget = Mathf.Sqrt(-2 * launchSpeedY / playerPhys.gravity) + Mathf.Sqrt(2 * (bh.velocity.y - launchSpeedY) / playerPhys.gravity);        
        Vector3 previousDrawPoint = bh.transform.position;

        int resolution = 30;
        for (int i = 0; i < resolution; i++) 
        {
            Debug.Log(i);
            float simulationTime = i / (float)resolution * timeToTarget;
            Vector3 displacement = bh.velocity * simulationTime +
            playerPhys.gravity * Vector3.down /*vector3.down??*/ //* simulationTime * simulationTime / 2f;
            /*Vector3 drawPoint = bh.transform.position + displacement;
            Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
            previousDrawPoint = drawPoint; 
        }
    }*/

}


