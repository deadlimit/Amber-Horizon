using System.Collections;
using System.Numerics;
using JetBrains.Annotations;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Controller3D : MonoBehaviour
{
    public Camera cam;
    [SerializeField] Vector3 velocity = Vector3.zero;
    [SerializeField] float acceleration = 4f;
    [SerializeField]float maxSpeed;

    public float launchSpeed = 5f;
    public float jumpHeight = 4f;
    Vector3 input = Vector3.zero;
    public PhysicsComponent playerPhys;
    
    [Header("Dash")]
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashLength;
    [Tooltip("Hur länge spelarens gravitation är avstängd innan den sätts på igen (sekunder)")]
    [SerializeField] private float timeWithoutGravity;
    [SerializeField] private float blackHoleGravityDashForce;
    private float nextDash;

    private Animator effects;
    
    
    [Header("StateMachine")]
    public State[] states;
    private StateMachine stateMachine;
    private bool jump;

    public BlackHole blackHole;

    void Awake()
    {
        playerPhys = GetComponent<PhysicsComponent>();
        stateMachine = new StateMachine(this, states);
        effects = GetComponent<Animator>();
    }

    public void SetInput(Vector3 inp)
    {
        input = inp;
        
    }

    private void RotateTowardsCameraDirection() {

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, cam.transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
    void PlayerDirection() {     
        input = cam.transform.rotation * input;
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

        if (Input.GetKeyDown(KeyCode.LeftShift))
            LaunchBH();

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

        Vector3 cameraForwardDirection = cam.transform.forward;

        //Nollar y-axeln för att bara dasha framåt.
        cameraForwardDirection.y = 0;
        
        //Stänger av gravitationen och nollställer hastigheten för att endast dash-velociteten ska gälla. 
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
    }
    
    private void LaunchBH() 
    {
        
        BlackHole bh;
        bh = Instantiate(blackHole, transform.position, Quaternion.identity);
        bh.velocity = transform.TransformDirection(BHTrajectory() * launchSpeed);

    }
    private Vector3 BHTrajectory() 
    {
        return (cam.transform.forward + Vector3.up).normalized;
    }


}


