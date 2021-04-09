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

    private float mouseSense = 0f;
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
        mouseSense = mouseSensitivity;
    }

    void LateUpdate()
    {
        //stateMachine.RunUpdate();
        GetInput();
        CameraScroll();
        
        //rotationX = Mathf.Clamp(rotationX, -80, 85);
        offset = transform.rotation * cameraOffset;
        PlaceCamera();
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
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
        rotationX -= Input.GetAxisRaw("Mouse Y") * mouseSense;
        rotationY += Input.GetAxisRaw("Mouse X") * mouseSense;
        mouseSense = mouseSensitivity; 
    }
    void PlaceCamera() {     
        RaycastHit hitInfo;
        playerPos = player.transform.position;
        Debug.DrawLine(transform.position, transform.position + Vector3.down * offset.magnitude);


        if (Physics.SphereCast(playerPos, coll.radius, offset.normalized, out hitInfo, offset.magnitude, collisionMask))
        {
            Vector3 hitOffset = new Vector3(cameraOffset.x, hitInfo.point.y - coll.radius / 2, hitInfo.point.z - coll.radius / 2);
            offset = hitInfo.distance * offset.normalized;
            //stateMachine.ChangeState<CollisionCameraState>(); 
            
            if (groundCheck())
            {
                Debug.Log("cam grounded");
                mouseSense = mouseSensitivity * 0.1f;
                
            }
        }

        
            //stateMachine.ChangeState<DefaultCameraState>();
        transform.position = Vector3.Lerp(transform.position, playerPos + offset, camSpeed * Time.deltaTime);
        

        
    }

    private bool groundCheck()
    {
        return Physics.SphereCast(transform.position, coll.radius, Vector3.down, out RaycastHit hitInfo, coll.radius);
    }
}
