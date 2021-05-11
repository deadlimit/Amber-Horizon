using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "CameraFocusState", menuName = "New CameraFocusState")]
public class CameraFocusState : CameraBaseState  {
  /*  
    [SerializeField] private float distance;
    [SerializeField] private float zoomInSpeed;
    private ThirdPersonCamera camera;
    private Transform target;

    protected override void Initialize() {
        camera = owner as ThirdPersonCamera;
        target = GameObject.FindGameObjectWithTag("KeyCameraTarget").transform;
    }

    public override void Enter() {
        Target = target;
        Player.enabled = false;
    }

    public override void RunUpdate() {

        camera.transform.position = Vector3.Lerp(camera.transform.position, target.position + target.transform.forward * distance, Time.deltaTime * zoomInSpeed);

        //TODO Vafan gör du här
        if (Input.GetKeyDown(KeyCode.Tab)) {
            Player.enabled = true;
            stateMachine.ChangeState<DefaultCameraState>();
        }
            
        camera.transform.LookAt(target);
    }
*/
}
