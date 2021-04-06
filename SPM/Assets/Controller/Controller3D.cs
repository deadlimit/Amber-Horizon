using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller3D : MonoBehaviour
{
    public Camera cam;
    [SerializeField] Vector3 velocity = Vector3.zero;
    [SerializeField] float acceleration = 4f;
    [SerializeField]float maxSpeed;

    public float launchSpeed = 5f;
    public float jumpHeight = 4f;
    Vector3 input = Vector3.zero;
    PlayerPhysics playerPhys;

    public State[] states;
    private StateMachine stateMachine;
    private bool jump;

    public BlackHole blackHole;

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
        //input = GetInput().normalized;
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

    void Update()
    {
        stateMachine.RunUpdate();
        Debug.DrawLine(transform.position, transform.position + velocity, Color.red);
        PlayerDirection();
        playerPhys.HandleInput(velocity);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            LaunchBH();

    }
    public void SetJump()
    {
        jump = true;
    }


    public PlayerPhysics GetPhysics() { return playerPhys; }

    private void LaunchBH() 
    {
        BlackHole bh;
        bh = Instantiate(blackHole, transform.position, cam.transform.rotation);
        bh.velocity = transform.TransformDirection(Vector3.forward * launchSpeed);

    }


}


