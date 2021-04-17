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

    [Header("StateMachine")]
    public State[] states;
    private StateMachine stateMachine;
    private bool jump;
    float dot;

    private Vector3 force;
    private Vector3 input;
    public PhysicsComponent physics;
    private Camera activeCamera;
    private float smallNumber = 0.02f;
    void Awake()
    {
        activeCamera = Camera.main;
        physics = GetComponent<PhysicsComponent>();
        stateMachine = new StateMachine(this, states);
    }

    public void SetInput(Vector3 inp) 
    {
        input = inp;
    }

   public void SetInput(Vector3 inp, bool airborne) 
    {
        input = inp * airControl;
    }
    void Decelerate() 
    {
        
        Vector3 projection = new Vector3(physics.velocity.x, 0, physics.velocity.z).normalized;
        force = -deceleration * projection * Time.deltaTime;
        //Velocitys magnitud och riktning, multiplicerat med ett värde mellan 1 och 0, fast negativt
    }
    void Accelerate()
    {
        /*float dot = Vector3.Dot(input.normalized, physics.velocity.normalized);

        Debug.Log(dot);*/

        force = input * Time.deltaTime * acceleration;
       // input += ((dot - 1) * deceleration * -physics.velocity.normalized) / 2;                           
    }

    void PlayerDirection() 
    {
        input = activeCamera.transform.rotation * input;
        RotateTowardsCameraDirection();
        input = input.magnitude * Vector3.ProjectOnPlane(input, physics.groundHitInfo.normal).normalized;

    }
    void Jump() { if (jump) { input.y += jumpHeight; jump = false; }  }
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
            Decelerate();
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
