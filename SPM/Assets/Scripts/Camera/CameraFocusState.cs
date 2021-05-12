using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "CameraFocusState", menuName = "New CameraFocusState")]
public class CameraFocusState : State {

    private ThirdPersonCamera camera;

    public float Distance;
    public float ZoomInSpeed;
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

        camera.transform.position = Vector3.Lerp(camera.transform.position, target.position + target.transform.forward * Distance, Time.deltaTime * ZoomInSpeed);

        if (Input.GetKeyDown(KeyCode.Tab)) {
            player.enabled = true;
            stateMachine.ChangeState<DefaultCameraState>();
        }
            
        camera.transform.LookAt(target);
    }

}
