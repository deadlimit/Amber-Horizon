using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 1f;
    public float rotationX;
    public float rotationY;
    public float cameraHeight = 2f;
    public float cameraDistance = -4f;
    public GameObject player;
    public LayerMask collisionMask;
    public float skinWidth = 0.1f;
    public float camSpeed = 1f;

    public Vector3 playerPos;
    Vector3 cameraOffset;
    Vector3 offset = Vector3.zero;
    SphereCollider coll;
    void Start()
    {
        coll = GetComponent<SphereCollider>();
        cameraOffset = new Vector3(0, cameraHeight, cameraDistance);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {     
        GetInput();
        //rotationY =  Mathf.Clamp(rotationY, -90, 90);
        rotationX = Mathf.Clamp(rotationX, -80, 80);    
        PlaceCamera();
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
    }

    void GetInput()
    {
        rotationX -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        rotationY += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
    }
    void PlaceCamera() {     
        RaycastHit hitInfo;
        playerPos = player.transform.position;
        offset = transform.rotation * cameraOffset;
    
        if (Physics.SphereCast(playerPos, coll.radius, offset.normalized, out hitInfo, cameraOffset.magnitude, collisionMask))
        {
            //hitInfo transform är collidern vi krockar med. lite tokigt? 
            Vector3 hitOffset = new Vector3(cameraOffset.x, hitInfo.point.y + coll.radius / 2, hitInfo.point.z + coll.radius / 2);
            offset = transform.rotation * (hitOffset.normalized * hitInfo.distance);
        }
        Debug.DrawLine(playerPos, playerPos + offset  , Color.green);

        Vector3 lerp = Vector3.Lerp(transform.position, offset + playerPos, camSpeed * Time.deltaTime);
        transform.position = lerp;
    }
}
