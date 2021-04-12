using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CollisionCameraState : State
{
    ThirdPersonCamera camera;
    float collisionRotation;
    protected override void Initialize()
    {
        Debug.Log("CollisionState");
        camera = (ThirdPersonCamera)base.owner;
       
    }
    public override void RunUpdate()
    {
        collisionRotation = camera.rotationX;

        if (groundCheck())
        {
            Debug.Log("cam grounded");
            camera.rotationX = Mathf.Clamp(camera.rotationX, collisionRotation, collisionRotation + 25);
        }
        else
        {
            stateMachine.ChangeState<DefaultCameraState>();
        }

    }
    private bool groundCheck() 
    {
        return Physics.SphereCast(camera.transform.position, camera.coll.radius,Vector3.down, out RaycastHit hitInfo, camera.camSpeed * Time.deltaTime);
    }
}
