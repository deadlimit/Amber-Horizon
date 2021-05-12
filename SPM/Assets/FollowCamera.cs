using EventCallbacks;
using UnityEngine;

public class FollowCamera : CameraBehaviour {
    
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private Vector3 TargetOffset;
    [SerializeField] private Transform PlayerTarget;
    [SerializeField] private float MouseSensitivity;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float minZoom = -6f;
    [SerializeField] private float maxZoom = -2f;
    private Vector3 collisionOffset;
    
    private Vector2 rotation;
    private RaycastHit hitInfo;
    
    private string xRotation = "Mouse X";
    private string yRotation = "Mouse Y";
    
    
    public override void MovementBehaviour() {
        GetInput();
        CameraScroll();
        collisionOffset = ActiveCamera.transform.rotation * TargetOffset;
        PlaceCamera();

        rotation.x = Mathf.Clamp(rotation.x, -40, 80);
        ActiveCamera.transform.rotation = Quaternion.Euler(rotation.x - 10, rotation.y, 0);
    }
    
    private void CameraScroll() 
    {
        //eventuellt ska dessa clampas
        TargetOffset.z += Input.mouseScrollDelta.y; 
        TargetOffset.z = Mathf.Clamp(TargetOffset.z, minZoom, maxZoom);

    }
        
    void GetInput()
    {
        rotation.x -= Input.GetAxisRaw(yRotation) * MouseSensitivity;
        rotation.y += Input.GetAxisRaw(xRotation) * MouseSensitivity;
    }
        
    void PlaceCamera() {     
            
            
        if (Physics.SphereCast(PlayerTarget.position, Collider.radius, collisionOffset.normalized, out hitInfo, collisionOffset.magnitude, collisionMask)) {

            collisionOffset = hitInfo.distance * collisionOffset.normalized;
        }
            
        CameraTransform.position = Vector3.Lerp(ActiveCamera.transform.position, PlayerTarget.position + collisionOffset, cameraSpeed * Time.deltaTime);             
    }
    
}
