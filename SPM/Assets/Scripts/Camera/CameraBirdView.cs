using System.Numerics;
using EventCallbacks;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[CreateAssetMenu(fileName = "CameraBirdView State", menuName = "New CameraBirdView State")]
public class CameraBirdView : State {

    private Transform birdViewTransform;
    private ThirdPersonCamera cameraController;

    public float ZoomOutSpeed;
    
    protected override void Initialize() {
        cameraController = owner as ThirdPersonCamera;
        birdViewTransform = GameObject.FindGameObjectWithTag("TransitOverview").transform;
    }

    public override void Enter() {
        Camera.main.orthographic = true;
        EventSystem<ExitTransitView>.RegisterListener(CancelView);
    }

    public override void RunUpdate() {
        cameraController.transform.position = Vector3.Lerp(cameraController.transform.position, birdViewTransform.transform.position, Time.deltaTime * ZoomOutSpeed);
        cameraController.transform.rotation = Quaternion.Lerp(cameraController.transform.rotation, birdViewTransform.transform.rotation, Time.deltaTime * ZoomOutSpeed);

        if (Input.GetKeyDown(KeyCode.Escape)) {
            EventSystem<ExitTransitView>.FireEvent(null);
            stateMachine.ChangeState<DefaultCameraState>();
        }
            
    }


    private void CancelView(ExitTransitView view) {
        stateMachine.ChangeState<DefaultCameraState>();
    }

    public override void Exit() {
        EventSystem<ExitTransitView>.UnregisterListener(CancelView);
        Camera.main.orthographic = false;
    }
}
