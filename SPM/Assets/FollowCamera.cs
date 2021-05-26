using UnityEngine;

public class FollowCamera : CameraBehaviour {
    
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private Vector3 TargetOffset;
    [SerializeField] private Transform PlayerTarget;
    [SerializeField] private float MouseSensitivity;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    
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
            
        CameraTransform.position = Vector3.Lerp(ActiveCamera.transform.position, PlayerTarget.position + collisionOffset, cameraSpeed * Time.deltaTime);             
    }
    
}
