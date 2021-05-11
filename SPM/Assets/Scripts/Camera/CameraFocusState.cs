using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "CameraFocusState", menuName = "New CameraFocusState")]
public class CameraFocusState : State {
    
    [SerializeField] private float distance;
    [SerializeField] private float zoomInSpeed;
    private ThirdPersonCamera camera;
    private Transform target;
    private PlayerController player;
    
    protected override void Initialize() {
        camera = owner as ThirdPersonCamera;
        target = GameObject.FindGameObjectWithTag("KeyCameraTarget").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public override void Enter() {
        player.enabled = false;
    }

    public override void RunUpdate() {

        camera.transform.position = Vector3.Lerp(camera.transform.position, target.position + target.transform.forward * distance, Time.deltaTime * zoomInSpeed);

        //TODO Vafan gör du här
        if (Input.GetKeyDown(KeyCode.Tab)) {
            player.enabled = true;
            stateMachine.ChangeState<DefaultCameraState>();
        }
            
        camera.transform.LookAt(target);
    }

}
