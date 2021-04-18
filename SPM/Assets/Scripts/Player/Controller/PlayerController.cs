using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Player Control")]
    public float jumpHeight = 5f;
    public float acceleration = 5f;
    public float deceleration = 2f;
    public float airControl = 0.2f;
    public float maxSpeed = 5f;
    public float turnBerth = 4f;
    public float turnSpeed = 0.33f; 

    [Header("StateMachine")]
    public State[] states;
    private StateMachine stateMachine;
    private bool jump;
    float dot;

    private Vector3 force;
    private Vector3 input;
    public PhysicsComponent physics;
    private Camera activeCamera;
    public bool airborne;
    void Awake()
    {
        activeCamera = Camera.main;
        physics = GetComponent<PhysicsComponent>();
        stateMachine = new StateMachine(this, states);
    }

    public void SetInput(Vector3 inp) 
    {
        input = inp;
        airborne = false;
    }

   public void SetInput(Vector3 inp, bool airborne) 
    {
        input = inp * airControl;
        airborne = true;
    }
    void Decelerate() 
    {
        force = -deceleration * physics.GetXZMovement().normalized * Time.deltaTime;
        //Velocitys magnitud och riktning, multiplicerat med ett värde mellan 1 och 0, fast negativt
    }
    void Accelerate()
    {
        Vector3 inputXZ = new Vector3(input.x, 0, input.z);
        float dot = Vector3.Dot(inputXZ.normalized, physics.GetXZMovement().normalized);

        Debug.Log(dot);

        force = input * Time.deltaTime * acceleration;
        /*
        om vi accelerar i en annan riktning vill vi egentligen bromsa först
        skalärprodukten används i multiplikation för att avgöra hur mycket av decelerationen som ska
        appliceras, då detta bör bero på vinkeln i vilken man byter riktning/velocitet/momentum
        */
 
        force -= (((dot - 1) * turnBerth * -physics.GetXZMovement().normalized) / 2);
        //addera * turnSpeed av kraften vi precis tog bort, till vår nya riktning.
        //gör i princip att man svänger snabbare
        force += (((dot - 1) * turnBerth * turnSpeed * -force.normalized) / 2) ;
        Debug.DrawLine(transform.position, transform.position + -((dot - 1) * turnBerth * -physics.velocity.normalized) / 2, Color.red);
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
    void Update()
    {
       
        stateMachine.RunUpdate();
        PlayerDirection();

        if (input.magnitude < float.Epsilon)
        {
            Decelerate();
        }
        else
            Accelerate(); 
        //Debug.Log(input);
        Jump();
        physics.AddForce(force);
        
       /* if (Input.GetKeyDown(KeyCode.E) && nextDash < Time.time)
        {
            nextDash = Time.time + dashCooldown;
            StopCoroutine(Dash());
            StartCoroutine(Dash());
        }

        if (Input.GetMouseButtonDown(1))
            lbh.Activate();
        if (Input.GetMouseButtonUp(1))
            lbh.Deactivate();*/
    }
}
