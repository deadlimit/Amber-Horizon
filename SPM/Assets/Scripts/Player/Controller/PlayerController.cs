using System;
using AbilitySystem;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [Header("Player Control")]
    public float jumpHeight = 5f;
    public float acceleration = 5f;
    public float deceleration = 2f;
    public float airControl = 0.2f;
    public float maxSpeed = 5f;
    public float turnRate = 4f;
    public float retainedSpeedWhenTurning = 0.33f; 

    [Header("StateMachine")]
    public State[] states;
    private StateMachine stateMachine;
    private bool jump;
    

    [HideInInspector] public Vector3 force;
     public Vector3 bhVelocity;
    private Vector3 input;
    public PhysicsComponent physics { get; private set; }
    private Camera activeCamera;
    public bool airborne;
    private LineRenderer lr;
    
    private GameplayAbilitySystem abilitySystem;
    
    void Awake()
    {
        activeCamera = Camera.main;
        physics = GetComponent<PhysicsComponent>();
        stateMachine = new StateMachine(this, states);       
        lr = GetComponent<LineRenderer>();
    }

    private void Start() {
        abilitySystem = gameObject.GetComponent<GameplayAbilitySystem>();
    }
    public void InputGrounded(Vector3 inp) 
    {
        input = inp;
        PlayerDirection();
        
        if (input.magnitude < float.Epsilon)
        {
            Decelerate();
        }
        else
            Accelerate();
        airborne = false;
    }


   public void InputAirborne(Vector3 inp, bool airborne) 
    {
        input = inp * airControl;
        PlayerDirection();
        AccelerateAirborne();
        airborne = true;
    }
    void Decelerate() 
    {
        //viktigt att notera att jag inte riktigt vet varför detta typ(!) fungerar
        MovingPlatformV2 mp = physics.groundHitInfo.collider.gameObject.GetComponent<MovingPlatformV2>();
        if (mp)
        {
            force = deceleration * mp.GetVelocity().normalized * Time.deltaTime;
            force += -deceleration * physics.GetXZMovement().normalized * Time.deltaTime;
        }
        else
            force = -deceleration * physics.GetXZMovement().normalized * Time.deltaTime;
        //Velocitys magnitud och riktning, multiplicerat med ett v�rde mellan 1 och 0, fast negativt
    }
    void Accelerate()
    {
        Vector3 inputXZ = new Vector3(input.x, 0, input.z);
        float dot = Vector3.Dot(inputXZ.normalized, physics.GetXZMovement().normalized);
        
        force = input * Time.deltaTime * acceleration;
        /*
        om vi accelerar i en annan riktning vill vi egentligen bromsa f�rst
        skal�rprodukten anv�nds i multiplikation f�r att avg�ra hur mycket av decelerationen som ska
        appliceras, d� detta b�r bero p� vinkeln i vilken man byter riktning/velocitet/momentum
        */
 
        force -= (((dot - 1) * turnRate * -physics.GetXZMovement().normalized) / 2);
        //addera * turnSpeed av kraften vi precis tog bort, till v�r nya riktning.
        //g�r i princip att man sv�nger snabbare
        force += (((dot - 1) * turnRate * retainedSpeedWhenTurning * -force.normalized) / 2) ;
        Debug.DrawLine(transform.position, transform.position + -((dot - 1) * turnRate * -physics.velocity.normalized) / 2, Color.red);
    }


    private void AccelerateAirborne()
    {
        force = input * Time.deltaTime * acceleration;
    }

    void PlayerDirection() 
    {
        input = activeCamera.transform.rotation * input;
        RotateTowardsCameraDirection();
        input = input.magnitude * Vector3.ProjectOnPlane(input, physics.groundHitInfo.normal).normalized;

    }
    void Jump() { if (jump) { force.y += jumpHeight; jump = false; }  }
    public void SetJump()
    {
        jump = true;
    }
    void RotateTowardsCameraDirection() 
    {
            transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x, 
            activeCamera.transform.localEulerAngles.y, 
            transform.localEulerAngles.z);
    }

    void Update() {

        physics.isGrounded();
        stateMachine.RunUpdate();
        Jump();

        if (Input.GetKeyDown(KeyCode.E)) 
        {
            print(abilitySystem.TryActivateAbilityByTag(GameplayTags.MovementAbilityTag));
        }
          
        if (Input.GetMouseButton(1))
        {
            abilitySystem.TryActivateAbilityByTag(GameplayTags.AimingTag);
        }

        if (Input.GetMouseButtonUp(1))
        {
            abilitySystem.TryDeactivateAbilityByTag(GameplayTags.AimingTag);
        }
        if (Input.GetMouseButton(0))
        {
            abilitySystem.TryActivateAbilityByTag(GameplayTags.BlackHoleAbilityTag);
        }

        physics.AddForce(force);
        
        
    }

    public void OnDisable() {
        print("disable");
    }
}
