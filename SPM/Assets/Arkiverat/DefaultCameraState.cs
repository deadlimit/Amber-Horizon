using EventCallbacks;
using UnityEngine;


public class DefaultCameraState : State
{
    public LayerMask CollisionMask;
    public float CameraSpeed;
    public float MouseSensitivity = 1f;
    private Vector3 playerPos;
    private ThirdPersonCamera camera;
    private GameObject target;
    private  float rotationX;
    private float rotationY;
    public Vector3 TargetOffset;
    private Vector3 offset;
    public SphereCollider collider { get; private set; }
    
    
    protected override void Initialize() {
        camera = (ThirdPersonCamera)base.owner;
        collider = camera.GetComponent<SphereCollider>();
        target = GameObject.FindGameObjectWithTag("CameraDefaultTarget");
        EventSystem<EnterTransitViewEvent>.RegisterListener(ChangeToBirdEyeView);

    }
    public override void RunUpdate() 
    {
        if(Input.GetKeyDown(KeyCode.Tab))
            stateMachine.ChangeState<CameraFocusState>();
        
        GetInput();
        CameraScroll();
        
        offset = camera.transform.rotation * TargetOffset;
        PlaceCamera();

        rotationX = Mathf.Clamp(rotationX, -40, 80);
        camera.transform.rotation = Quaternion.Euler(rotationX - 10, rotationY, 0);

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


        if (Physics.SphereCast(playerPos, collider.radius, offset.normalized, out hitInfo, offset.magnitude, CollisionMask)) {

            offset = hitInfo.distance * offset.normalized;
        }
        
        
        camera.transform.position = Vector3.Lerp(camera.transform.position, playerPos + offset, CameraSpeed * Time.deltaTime);             
    }

    private void ChangeToBirdEyeView(EnterTransitViewEvent viewEvent) {
        target = viewEvent.TransitCameraFocusInfo.NewFocusTarget.gameObject;
        offset = viewEvent.TransitCameraFocusInfo.NewOffset;
    }
    
    
    
}
