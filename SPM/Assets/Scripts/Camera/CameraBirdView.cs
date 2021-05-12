using System.Numerics;
using EventCallbacks;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[CreateAssetMenu(fileName = "CameraBirdView State", menuName = "New CameraBirdView State")]
public class CameraBirdView : State {

    private Transform birdViewTransform;
    private ThirdPersonCamera cameraController;

    [SerializeField] private float zoomOutSpeed;

    private Camera camera;
    
    //TODO 3D-view vid checkpointen istället för att lerpa upp i luften?
    protected override void Initialize() {
        cameraController = owner as ThirdPersonCamera;
        birdViewTransform = GameObject.FindGameObjectWithTag("TransitOverview").transform;
        camera = Camera.main;
    }

    public override void Enter() {
        camera.orthographic = true;
        EventSystem<ExitTransitViewEvent>.RegisterListener(CancelView);
    }

    public override void RunUpdate() {
        cameraController.transform.position = Vector3.Lerp(cameraController.transform.position, birdViewTransform.transform.position, Time.deltaTime * zoomOutSpeed);
        cameraController.transform.rotation = Quaternion.Lerp(cameraController.transform.rotation, birdViewTransform.transform.rotation, Time.deltaTime * zoomOutSpeed);

        if (Input.GetKeyDown(KeyCode.Escape)) {
            EventSystem<ExitTransitViewEvent>.FireEvent(null);
            stateMachine.ChangeState<DefaultCameraState>();
        }
            
    }


    private void CancelView(ExitTransitViewEvent viewEvent) {
        stateMachine.ChangeState<DefaultCameraState>();
    }

    public override void Exit() {
        EventSystem<ExitTransitViewEvent>.UnregisterListener(CancelView);
        Camera.main.orthographic = false;
    }
    

}
