using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class Controller3D : MonoBehaviour
{
    public Camera cam;
    [SerializeField] Vector3 velocity = Vector3.zero;
    [SerializeField] float acceleration = 4f;
    [SerializeField] float maxSpeed;
    public float jumpHeight = 4f;
    Vector3 input = Vector3.zero;
    PlayerPhysics playerPhys;
    
    [Header("Dash")]
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashLength;
    [Tooltip("Hur länge spelarens gravitation är avstängd innan den sätts på igen (sekunder)")]
    [SerializeField] private float timeWithoutGravity;
    [SerializeField] private float dissolveSpeed;
    
    private float nextDash;

    
    [Header("StateMachine")]
    public State[] states;
    private StateMachine stateMachine;
    private bool jump;

    void Awake()
    {
        playerPhys = GetComponent<PlayerPhysics>();
        stateMachine = new StateMachine(this, states);
    }

    public void SetInput(Vector3 inp)
    {
        input = inp;
        
    }
    void PlayerDirection() {
        
        input = cam.transform.rotation * input;
        Vector3 angle = Vector3.ProjectOnPlane(input, playerPhys.groundHitInfo.normal).normalized;
        input += angle * input.magnitude;
        Accelerate();
    }
    private void Accelerate()
    {
        //tror det är clamp som hindrar gravitationen att appliceras när man hoppar intill en vägg och sedan rör sig därifrån..? 

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

        if (Input.GetKeyDown(KeyCode.F)) {
            Time.timeScale = Time.timeScale == .3f ? Time.timeScale = 1 : Time.timeScale = .3f;
        }
        
        stateMachine.RunUpdate();
        Debug.DrawLine(transform.position, transform.position + velocity, Color.red);
        PlayerDirection();
        
        playerPhys.HandleInput(velocity);
        
        
        if (Input.GetKeyDown(KeyCode.E) && nextDash < Time.time) {
            nextDash = Time.time + dashCooldown;
            StartCoroutine(Dash());
        }

    }
    public void SetJump()
    {
        jump = true;
    }

    
    public PlayerPhysics GetPhysics() { return playerPhys; }
    
    /// <summary>
    /// Dash. Ska kunna dasha åt input-hållet? Dashar endast rakt fram just nu. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Dash() {

        StartCoroutine(MaterialManipulator.Dissolve(GetComponent<MeshRenderer>().material, timeWithoutGravity, dissolveSpeed));
        
        //Spara gravitationen innan man sätter den till 0
        float gravity = playerPhys.gravity;

        Vector3 cameraForwardDirection = cam.transform.forward;

        //Nollar y-axeln för att bara dasha framåt.
        cameraForwardDirection.y = 0;
        
        //Stänger av gravitationen och nollställer hastigheten för att endast dash-velociteten ska gälla. 
        playerPhys.velocity = Vector3.zero;
        playerPhys.gravity = 0;
        
        
        velocity = cameraForwardDirection * dashLength;
        playerPhys.HandleInput(velocity);

        //Vänta .4 sekunder innan man sätter på gravitationen igen. 
        yield return new WaitForSeconds(timeWithoutGravity);

        playerPhys.velocity = transform.forward * 2;
        playerPhys.gravity = gravity;

    }
    

}


