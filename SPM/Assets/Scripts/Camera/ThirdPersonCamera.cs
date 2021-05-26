using EventCallbacks;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {
    
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private Vector3 TargetOffset;
    [SerializeField] private Transform PlayerTarget;
    [SerializeField] private float MouseSensitivity;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    
    private Transform Target;
    private Camera ActiveCamera;
    private SphereCollider Collider;

    
    private Vector3 collisionOffset;
    private Vector2 rotation;
    private RaycastHit hitInfo;
    
    private static readonly string xRotation = "Mouse X";
    private static readonly string yRotation = "Mouse Y";

    private void Awake() {
        Cursor.ActivateCursor(false, CursorLockMode.Locked);
        ActiveCamera = Camera.main;
        Collider = GetComponent<SphereCollider>();
    }
    
    public void LateUpdate() {
        GetInput();
        CameraScroll();
        collisionOffset = transform.rotation * TargetOffset;
        PlaceCamera();

        rotation.x = Mathf.Clamp(rotation.x, -40, 80);
        transform.rotation = Quaternion.Euler(rotation.x - 10, rotation.y, 0);
    }
    
    private void CameraScroll() 
    {
        TargetOffset.z += Input.mouseScrollDelta.y; 
        TargetOffset.z = Mathf.Clamp(TargetOffset.z, minZoom, maxZoom);

    }
        
    private void GetInput()
    {
        rotation.x -= Input.GetAxisRaw(yRotation) * MouseSensitivity;
        rotation.y += Input.GetAxisRaw(xRotation) * MouseSensitivity;
    }
        
    private void PlaceCamera() {     
            
            
        if (Physics.SphereCast(PlayerTarget.position, Collider.radius, collisionOffset.normalized, out hitInfo, collisionOffset.magnitude, collisionMask)) {

            collisionOffset = hitInfo.distance * collisionOffset.normalized;
        }
            
        transform.position = Vector3.Lerp(ActiveCamera.transform.position, PlayerTarget.position + collisionOffset, cameraSpeed * Time.deltaTime);             
    }
    

    
    
}
