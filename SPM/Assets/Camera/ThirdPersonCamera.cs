using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public GameObject player;
    public float mouseSensitivity = 1f;
    public float camSpeed = 1f;
    public LayerMask collisionMask;
    public float cameraHeight = 2f;
    public float cameraDistance = -4f;
    
    Vector3 playerPos;
    Vector3 cameraOffset;
    Vector3 offset = Vector3.zero;
    [HideInInspector]public SphereCollider coll;
    public float rotationX;
    public float rotationY;

   /* public State[] states; 
    private StateMachine stateMachine;*/
    void Awake()
    {
        //stateMachine = new StateMachine(this, states);

        coll = GetComponent<SphereCollider>();
        cameraOffset = new Vector3(0, cameraHeight, cameraDistance);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        //stateMachine.RunUpdate();
        GetInput();
        CameraScroll();
        
        //rotationX = Mathf.Clamp(rotationX, -80, 85);
        offset = transform.rotation * cameraOffset;
        PlaceCamera();

        //magic number här, roterar kameran ytterligare lite nedåt, tyckte att det blev lättare då
        transform.rotation = Quaternion.Euler(rotationX - 10, rotationY, 0);
    }

    private void CameraScroll() 
    {
        //eventuellt ska dessa clampas
        float multiplier = cameraDistance / cameraHeight;
        cameraOffset.z += Input.mouseScrollDelta.y;
        cameraOffset.y += Input.mouseScrollDelta.y / multiplier;
        
    }
    void GetInput()
    {
        rotationX -= Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        rotationY += Input.GetAxisRaw("Mouse X") * mouseSensitivity;
    }
    void PlaceCamera() {     
        RaycastHit hitInfo;
        playerPos = player.transform.position;
        Debug.DrawLine(transform.position, transform.position + Vector3.down * coll.radius);


        if (Physics.SphereCast(playerPos, coll.radius, offset.normalized, out hitInfo, offset.magnitude, collisionMask))
        {
            offset = hitInfo.distance * offset.normalized; 
            
            if (groundCheck())
            {
                //magic number på slutet är bara till för att sakta ned kameran när man slår i marken
                //rätt fult ibland om kameran släpar i marken när karaktären svänger, men relativt litet problem
                transform.position = Vector3.Lerp(transform.position, playerPos + offset, camSpeed * Time.deltaTime * 0.15f) ;
                return;
            }
        }
        transform.position = Vector3.Lerp(transform.position, playerPos + offset, camSpeed * Time.deltaTime);             
    }

    private bool groundCheck()
    {
        return Physics.SphereCast(transform.position, coll.radius, Vector3.down, out RaycastHit hitInfo, offset.magnitude);
    }
}
