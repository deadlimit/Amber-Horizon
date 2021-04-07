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
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<SphereCollider>();
        cameraOffset = new Vector3(0, cameraHeight, cameraDistance);
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
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
       // Debug.DrawLine(playerPos, playerPos + offset  , Color.green);
        RaycastHit hitInfo;
        playerPos = player.transform.position;
        offset = transform.rotation * cameraOffset;
     
        //kollision.. på något sätt.
        if (Physics.SphereCast(playerPos, coll.radius, (playerPos + offset) - playerPos, out hitInfo, cameraOffset.magnitude, collisionMask))
        {

            Vector3 hitOffset = new Vector3(cameraOffset.x, hitInfo.transform.position.y + coll.radius / 2, hitInfo.transform.position.z + coll.radius / 2);
            offset = transform.rotation * (hitOffset.normalized * hitInfo.distance);

        }

        Vector3 lerp = Vector3.Lerp(transform.position, offset + playerPos, camSpeed * Time.deltaTime);
        transform.position = lerp;
    }
}
