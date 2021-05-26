using System.Runtime.InteropServices;
using EventCallbacks;
using UnityEngine;


public class DefaultCameraState : CameraBaseState
{
   /* [SerializeField] private LayerMask CollisionMask;
    [SerializeField] private Vector3 TargetOffset;
    public float CameraSpeed;
 

    
    private Vector3 offset;
    
    protected override void Initialize() {
        base.Initialize();
        EventSystem<EnterTransitViewEvent>.RegisterListener(ChangeToBirdEyeView);
        internalTargetReference = GameObject.FindGameObjectWithTag("CameraDefaultTarget").transform;
    }

    public override void Enter() {
        Debug.Log("enter");
        Target = internalTargetReference;
        Offset = TargetOffset;
    }
    public override void RunUpdate() 
    {
        if(Input.GetKeyDown(KeyCode.Tab))
            stateMachine.ChangeState<CameraFocusState>();
        
        GetInput();
        CameraScroll();
        
        offset = Camera.transform.rotation * Offset;
        PlaceCamera();

        rotationX = Mathf.Clamp(rotationX, -40, 80);
        Camera.main.transform.rotation = Quaternion.Euler(rotationX - 10, rotationY, 0);

    }
    private void CameraScroll() 
    {
        //eventuellt ska dessa clampas
        Offset.z += Input.mouseScrollDelta.y; 
        Offset.z = Mathf.Clamp(TargetOffset.z, -6, -2);

    }
    void GetInput()
    {
        rotationX -= Input.GetAxisRaw("Mouse Y") * MouseSensitivity;
        rotationY += Input.GetAxisRaw("Mouse X") * MouseSensitivity;
    }
    void PlaceCamera() {     
        RaycastHit hitInfo;
        
        if (Physics.SphereCast(Target.position, Collider.radius, offset.normalized, out hitInfo, offset.magnitude, CollisionMask)) {

            offset = hitInfo.distance * offset.normalized;
        }
        
        
        CameraController.transform.position = Vector3.Lerp(CameraController.transform.position, Target.position + offset, CameraSpeed * Time.deltaTime);             
    }

    private void ChangeToBirdEyeView(EnterTransitViewEvent viewEvent) {
        Target = viewEvent.TransitCameraFocusInfo.NewFocusTarget;
        Offset = viewEvent.TransitCameraFocusInfo.NewOffset;
    }
    
    */
    
}
