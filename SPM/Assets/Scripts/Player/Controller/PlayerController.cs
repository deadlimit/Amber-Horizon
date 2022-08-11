using System;
using AbilitySystem;
using EventCallbacks;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    #region Parameters exposed in the inspector
    [Header("Player Control")]
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 2f;
    [SerializeField] private float airControl = 0.2f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float turnRate = 4f;
    [SerializeField] private float retainedSpeedWhenTurning = 0.33f;

    [Header("GroundCheck")]
    [SerializeField] private LayerMask groundCheckMask;
    [SerializeField] private float groundCheckDistance = 0.05f;
    private BoxCollider groundCheckBox;

    [Header("SFX")]
    [SerializeField] private AudioClip blackHoleLaunchSound;

    [Header("StateMachine")]
    [SerializeField] private State[] states;
    private StateMachine stateMachine;
    #endregion

    [SerializeField] private Transform keyLookAtTarget;
    //have an array if this works.
    [SerializeField] private Vector3 levelOneStartPosition, levelTwoStartPosition;
    [HideInInspector] public Vector3 force;

    //Component references
    public PhysicsComponent physics { get; private set; }
    public Animator animator { get; private set; }
    private AudioSource audioSource;    
    private GameplayAbilitySystem abilitySystem;

    private Vector3 input;
    private bool jump;
    private Transform cameraTransform;
    private RaycastHit groundHitInfo;
    private bool wasGrounded;
    
    void Awake() 
    {
        string levelToLoad = PlayerPrefs.GetString("levelToLoad");
        Debug.Log("in PlayerController, Awake. levelToLoad is: " + levelToLoad);

        switch (levelToLoad)
        {
            case "Intro Cutscene":
                
                //do nothing as the player isn't in Intro
                break;
            case "Level 1 V2":
                //yield return SceneManager.LoadSceneAsync("Level 1 V2", LoadSceneMode.Additive);             
                transform.position = levelOneStartPosition;
                break;
            case "Level 2 V2":
                transform.position = levelTwoStartPosition;
                break;
        }

        cameraTransform = Camera.main.transform;
        physics = GetComponent<PhysicsComponent>();
        stateMachine = new StateMachine(this, states);       
        animator = GetComponent<Animator>();
        groundCheckBox = GetComponentInChildren<BoxCollider>();
        audioSource = GetComponent<AudioSource>();
        EventSystem<CheckPointActivatedEvent>.RegisterListener(CheckpointRestoreHealth);
    }

    private void Start() 
    {
        abilitySystem = gameObject.GetComponent<GameplayAbilitySystem>();
        //EventSystem<ActivatePlayerControl>.FireEvent(new ActivatePlayerControl(true));
    }
    private void Update() {       
        stateMachine.RunUpdate();

        if (Input.GetKeyDown(KeyCode.LeftShift) && wasGrounded)
        {
            if (abilitySystem.TryActivateAbilityByTag(GameplayTags.MovementAbilityTag))
                wasGrounded = false; 
        }
        
        if (Input.GetMouseButton(1))
        {
            abilitySystem.TryActivateAbilityByTag(GameplayTags.AimingTag);
        }

        if (Input.GetMouseButtonUp(1))
        {
            DeactivateAim();
        }
        if (Input.GetMouseButton(0))
        {
            if (abilitySystem.TryActivateAbilityByTag(GameplayTags.BlackHoleAbilityTag))
                audioSource.PlayOneShot(blackHoleLaunchSound, 5f);

        }        
    }
    private void FixedUpdate()
    {
        Jump();
        physics.AddForce(force);
        force = Vector3.zero;
    }

    #region Movement
    public void InputGrounded(Vector3 inp) 
    {
        input = inp;
        if (input.magnitude > 1f)
        {
            input.Normalize();
        }
        PlayerDirection();
        
        if (input.magnitude < float.Epsilon)
        {
            Decelerate();
        }
        else
            Accelerate();
    }
    public void InputAirborne(Vector3 inp, bool airborne) 
    {
        input = inp.normalized * airControl;
        PlayerDirection();
        AccelerateAirborne();
    }
    private void Accelerate()
    {
        Vector3 inputXZ = new Vector3(input.x, 0, input.z);
        float dot = Vector3.Dot(inputXZ.normalized, physics.GetXZMovement().normalized);
        
        force = input * acceleration;
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
        force = input * acceleration;
    }
    private void Decelerate() 
    {
        MovingPlatform mp = groundHitInfo.collider?.GetComponent<MovingPlatform>();

        if (mp)
        {
            force = deceleration * mp.GetVelocity().normalized;
            force += -deceleration * physics.GetXZMovement().normalized;
        }
        else
            force = -deceleration * physics.GetXZMovement().normalized;
        //Velocitys magnitud och riktning, multiplicerat med ett v�rde mellan 1 och 0, fast negativt
        /*
         *Frågan är om det faktiskt är en bättre idé att sköta plattformens påverkan på spelaren genom direkt positionsändring, istället för 
         * att göra det genom velociteten. Tekniskt sett mindre korrekt, men de blir mindre sammankopplade på det sättet, och det svåra med deceleration 
         * 
         */

    }
    private void PlayerDirection() 
    {
        Vector3 temp = cameraTransform.rotation.eulerAngles;
        temp.x = 0;
        Quaternion camRotation = Quaternion.Euler(temp);

        input = camRotation * input;
        input.y = 0;
        input = input.magnitude * Vector3.ProjectOnPlane(input, physics.groundHitInfo.normal).normalized;
        RotateTowardsCameraDirection();

    }
    private void Jump() 
    {
        if (jump) 
        { 
            force.y += jumpHeight / Time.fixedDeltaTime; jump = false; 
        } 
    }
    private void RotateTowardsCameraDirection() 
    {
        if (stateMachine.CurrentState.GetType() == typeof(PlayerLockedState))
            return;
        
        transform.localEulerAngles = new Vector3(
        transform.localEulerAngles.x,
        cameraTransform.transform.localEulerAngles.y, 
        transform.localEulerAngles.z);
    }
    #endregion


    
    /// <summary>
    /// Boxcast to get a little thickness to the groundcheck so as to not get stuck in crevasses or similar geometry. 
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded() {
        //wasGrounded used to prohibit consecutive dashes without touching ground

        if (Physics.BoxCast(transform.position + Vector3.up, groundCheckBox.size, Vector3.down, out groundHitInfo, transform.rotation, groundCheckDistance, groundCheckMask))
            wasGrounded = true;
        
        //Old groundcheck, kept in case the boxcast misbehaves. 
        //Physics.Raycast(transform.position, Vector3.down, out groundHitInfo, groundCheckDistance, groundCheckMask);       

        return groundHitInfo.collider;    
    }

    //Ability System
    private void CheckpointRestoreHealth(CheckPointActivatedEvent checkPointActivatedEvent)
    {
        RestoreHealth();
    }
    public void RestoreHealth()
    {
        abilitySystem.TryActivateAbilityByTag(GameplayTags.HealthRestoreTag);
    }
    public void DeactivateAim()
    {
        abilitySystem.TryDeactivateAbilityByTag(GameplayTags.AimingTag);
    }

    //Gets & Sets
    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
    public float GetPlayerHealth()
    {
        return (float)abilitySystem.GetAttributeValue(typeof(HealthAttribute));
    }
    public void SetJump()
    {
        jump = true;
    }
}
