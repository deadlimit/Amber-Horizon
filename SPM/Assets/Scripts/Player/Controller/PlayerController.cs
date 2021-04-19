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
    float dot;

    public GameplayAbility aim; 

    [HideInInspector]public Vector3 force;
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
        abilitySystem = gameObject.AddComponent<GameplayAbilitySystem>();
        abilitySystem.GrantAbility(aim);
        lr = GetComponent<LineRenderer>();
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

        stateMachine.RunUpdate();
        //PlayerDirection();

        //if (input.magnitude < float.Epsilon)
        //{
        //    Decelerate();
        //}
        //else
        //    Accelerate(); 
        //Debug.Log(input);
        Jump();

        if (Input.GetKeyDown(KeyCode.E)) {
            abilitySystem.TryActivateAbilityByTag(GameplayTags.MovementAbilityTag);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Försöker aktivera BH"); 
            abilitySystem.TryActivateAbilityByTag(GameplayTags.BlackHoleAbilityTag);
        }
        if (Input.GetMouseButton(1))
        {
            Debug.Log("Försöker sikta");
            abilitySystem.TryActivateAbilityByTag(GameplayTags.AimingTag);
        }
        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("Tar bort tag aiming?");
            abilitySystem.RemoveTag(aim.AbilityTag);
            lr.enabled = false;
        }

        physics.AddForce(force);
        
        
    }
}
