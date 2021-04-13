using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DefaultCameraState : State
{
    Vector3 playerPos;
    ThirdPersonCamera camera;
    protected override void Initialize()
    {
        Debug.Log("Default state");
        camera = (ThirdPersonCamera)base.owner;
    }
    public override void RunUpdate() 
    {
      // camera.rotationX = Mathf.Clamp(camera.rotationX, -120, 120);

    }


}
