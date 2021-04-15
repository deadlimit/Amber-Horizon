using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public float MouseSensitivity = 1f;
    public float CameraSpeed;
    public LayerMask CollisionMask;
    public Vector3 TargetOffset;
    
    public SphereCollider coll { get; private set; }

    public Transform target;
    private Vector3 playerPos;
    private Vector3 cameraOffset;
    private Vector3 offset;
    private  float rotationX;
    private float rotationY;
    
    void Awake() {
        
        coll = GetComponent<SphereCollider>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        GetInput();
        CameraScroll();
        
        offset = transform.rotation * TargetOffset;
        PlaceCamera();

        //magic number h�r, roterar kameran ytterligare lite ned�t, tyckte att det blev l�ttare d�
        rotationX = Mathf.Clamp(rotationX, -40, 80);
        rotationY = Mathf.Clamp(rotationY, -200, 200);
        transform.rotation = Quaternion.Euler(rotationX - 10, rotationY, 0);
    }

    private void CameraScroll() 
    {
        //eventuellt ska dessa clampas
        TargetOffset.z += Input.mouseScrollDelta.y; 
        TargetOffset.z = Mathf.Clamp(TargetOffset.z, -6, -2);

    }
    void GetInput()
    {
        rotationX -= Input.GetAxisRaw("Mouse Y") * MouseSensitivity;
        rotationY += Input.GetAxisRaw("Mouse X") * MouseSensitivity;
    }
    void PlaceCamera() {     
        RaycastHit hitInfo;
        playerPos = target.transform.position;
        Debug.DrawLine(transform.position, transform.position + Vector3.down * coll.radius);


        if (Physics.SphereCast(playerPos, coll.radius, offset.normalized, out hitInfo, offset.magnitude, CollisionMask))
        {
            offset = hitInfo.distance * offset.normalized; 
            
            if (groundCheck())
            {
                //magic number p� slutet �r bara till f�r att sakta ned kameran n�r man sl�r i marken
                //r�tt fult ibland om kameran sl�par i marken n�r karakt�ren sv�nger, men relativt litet problem
                transform.position = Vector3.Lerp(transform.position, playerPos + offset, CameraSpeed * Time.deltaTime * 0.15f) ;
                return;
            }
        }
        transform.position = Vector3.Lerp(transform.position, playerPos + offset, CameraSpeed * Time.deltaTime);             
    }

    private bool groundCheck()
    {
        return Physics.SphereCast(transform.position, coll.radius, -transform.up, out RaycastHit hitInfo, coll.radius);
    }
}
