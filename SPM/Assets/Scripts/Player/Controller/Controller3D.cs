using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Controller3D : MonoBehaviour
{
    private Camera activeCamera;
    [SerializeField] Vector3 velocity = Vector3.zero;
    [SerializeField] float acceleration = 4f;
    [SerializeField]float maxSpeed;

    public float jumpHeight = 4f;
    public Vector3 input = Vector3.zero;
    public PhysicsComponent playerPhys;
    public LauncherBlackHole lbh;
    public float deceleration = 1f;
    
    [Header("Dash")]
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashLength;
    [Tooltip("Hur länge spelarens gravitation är avstängd innan den sätts på igen (sekunder)")]
    [SerializeField] private float timeWithoutGravity;
    [SerializeField] private float blackHoleGravityDashForce;
    private float nextDash;
    [SerializeField] private int keyFragments = 0;

    private Animator effects;
    
    
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
        effects = GetComponent<Animator>();
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
            Debug.Log("Decelerating");
            Vector3 movementXZ = new Vector3(input.x, 0, input.z);
        if (movementXZ.magnitude < deceleration)
        {
            velocity.x = 0;
            velocity.z = 0;
        }
        else
        {
            playerPhys.AddForce(-deceleration  * Vector3.one);
        }
    }


    void Update() {

        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
        if (Input.GetKeyDown(KeyCode.F)) {
            Time.timeScale = Time.timeScale == .3f ? Time.timeScale = 1 : Time.timeScale = .3f;
        }
        
        stateMachine.RunUpdate();        
        PlayerDirection();     
        playerPhys.AddForce(velocity);
        
        if (Input.GetKeyDown(KeyCode.E) && nextDash < Time.time) {
            nextDash = Time.time + dashCooldown;
            StopCoroutine(Dash());
            StartCoroutine(Dash());
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
        
        effects.SetTrigger("Dash");
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
        playerPhys.velocity = forwardMomentum;
    }
    
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


