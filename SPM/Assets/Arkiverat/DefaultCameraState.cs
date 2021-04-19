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
        Debug.Log("Default state");
        target = GameObject.FindGameObjectWithTag("CameraDefaultTarget");
        
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
        Debug.DrawLine(camera.transform.position, camera.transform.position + Vector3.down * collider.radius);


        if (Physics.SphereCast(playerPos, collider.radius, offset.normalized, out hitInfo, offset.magnitude, CollisionMask))
        {
            offset = hitInfo.distance * offset.normalized; 
            
            if (groundCheck())
            {
                //magic number p� slutet �r bara till f�r att sakta ned kameran n�r man sl�r i marken
                //r�tt fult ibland om kameran sl�par i marken n�r karakt�ren sv�nger, men relativt litet problem
                camera.transform.position = Vector3.Lerp(camera.transform.position, playerPos + offset, CameraSpeed * Time.deltaTime * 0.15f) ;
                return;
            }
        }
        camera.transform.position = Vector3.Lerp(camera.transform.position, playerPos + offset, CameraSpeed * Time.deltaTime);             
    }

    private bool groundCheck()
    {
        return Physics.SphereCast(camera.transform.position, collider.radius, -camera.transform.up, out RaycastHit hitInfo, collider.radius);
    }


}
