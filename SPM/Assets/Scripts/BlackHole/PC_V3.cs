using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using AbilitySystem;


public class PC_V3 : MonoBehaviour
{

    [Header("Player Control")]
    public float jumpHeight = 5f;
    public float acceleration = 5f;
    public float deceleration = 2f;
    public float airControl = 0.2f;
    public float maxSpeed = 5f;
    public float turnRate = 4f;
    public float retainedSpeedWhenTurning = 0.33f;
    public LayerMask groundCheckMask;
    [Header("StateMachine")]
    public State[] states;
    private StateMachine stateMachine;
    private bool jump;
    public float groundCheckDistance = 0.05f;

    [HideInInspector] public Vector3 force;
    public Vector3 bhVelocity;
    private Vector3 input;
    public PhysicsComponent physics { get; private set; }
    private Camera activeCamera;
    public bool airborne;
    private LineRenderer lr;
    private RaycastHit groundHitInfo;
    public GameplayAbilitySystem abilitySystem { get; private set; }

    void Awake()
    {

        activeCamera = Camera.main;
        physics = GetComponent<PhysicsComponent>();
        stateMachine = new StateMachine(this, states);
        lr = GetComponent<LineRenderer>();

        EventSystem<CheckPointActivatedEvent>.RegisterListener(CheckpointRestoreHealth);
    }

    private void Start()
    {
        abilitySystem = gameObject.GetComponent<GameplayAbilitySystem>();
    }
    public void InputGrounded(Vector3 inp)
    {
        input = inp;
        if (input.magnitude > 1f)
        {
            Debug.Log("Grounded input normalized");
            input.Normalize();
        }

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
        input = inp.normalized * airControl;

        PlayerDirection();
        AccelerateAirborne();
        airborne = true;
    }
    void Decelerate()
    {
        MovingPlatform mp = groundHitInfo.collider?.GetComponent<MovingPlatform>();

        if (mp)
        {
            Debug.Log("On moving platform");
            force = deceleration * mp.GetVelocity().normalized;
            force += -deceleration * physics.GetXZMovement().normalized * Time.deltaTime;
        }
        else
            force = -deceleration * physics.GetXZMovement().normalized;
        //Velocitys magnitud och riktning, multiplicerat med ett v�rde mellan 1 och 0, fast negativt
    }
    void Accelerate()
    {
        Vector3 inputXZ = new Vector3(input.x, 0, input.z);
        float dot = Vector3.Dot(inputXZ.normalized, physics.GetXZMovement().normalized);

        force = input * acceleration * Time.deltaTime;
        /*
        om vi accelerar i en annan riktning vill vi egentligen bromsa f�rst
        skal�rprodukten anv�nds i multiplikation f�r att avg�ra hur mycket av decelerationen som ska
        appliceras, d� detta b�r bero p� vinkeln i vilken man byter riktning/velocitet/momentum
        */

        force -= (((dot - 1) * turnRate * -physics.GetXZMovement().normalized) / 2);
        //addera * turnSpeed av kraften vi precis tog bort, till v�r nya riktning.
        //g�r i princip att man sv�nger snabbare
        force += (((dot - 1) * turnRate * retainedSpeedWhenTurning * -force.normalized) / 2);
        Debug.DrawLine(transform.position, transform.position + -((dot - 1) * turnRate * -physics.velocity.normalized) / 2, Color.red);
    }


    private void AccelerateAirborne()
    {
        /*
         Force assignas här istället för att adderas, då blir resultatet beroende av multiplikationen med DT
        Men varför påverkas inte det som händer på marken?? Assignment till force där också ju 
        fixedDeltaTime gör att assignmenten alltid blir samma.. men då har vi antalet frames att ta in i beräkningen
         */
        force = input * acceleration * Time.deltaTime;
        Debug.Log(force);
    }

    void PlayerDirection()
    {
        input = activeCamera.transform.rotation * input;
        input.y = 0;
        Vector3 normal = isGrounded() ? groundHitInfo.normal : Vector3.up;
        RotateTowardsCameraDirection();
        input = input.magnitude * Vector3.ProjectOnPlane(input, physics.groundHitInfo.normal).normalized;

    }
    void Jump() { if (jump) { force.y += jumpHeight * Time.deltaTime; jump = false; } }
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

    void Update()
    {

        stateMachine.RunUpdate();
        Jump();

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

    private void FixedUpdate()
    {

    }
    public void RestoreHealth()
    {
        abilitySystem.TryActivateAbilityByTag(GameplayTags.HealthRestoreTag);
        Debug.Log("reached RestoreHealth, in PlayerCOntroller");
    }

    private void CheckpointRestoreHealth(CheckPointActivatedEvent checkPointActivatedEvent)
    {
        RestoreHealth();
    }


    public bool isGrounded()
    {

        Physics.Raycast(transform.position, Vector3.down, out groundHitInfo, groundCheckDistance, groundCheckMask);

        return groundHitInfo.collider;

        //groundHitInfo = collisionCaster.CastCollision(transform.position, Vector3.down, groundCheckDistance + skinWidth);

    }
    public float GetPlayerHealth()
    {
        return (float)abilitySystem.GetAttributeValue(typeof(HealthAttribute));
    }
}
