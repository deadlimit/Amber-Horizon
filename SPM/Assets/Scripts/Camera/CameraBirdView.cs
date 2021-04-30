using UnityEngine;

[CreateAssetMenu(fileName = "CameraBirdView State", menuName = "New CameraBirdView State")]
public class CameraBirdView : State {

    private Transform birdViewTransform;
    private ThirdPersonCamera cameraController;
    
    protected override void Initialize() {
        cameraController = owner as ThirdPersonCamera;
        birdViewTransform = GameObject.FindGameObjectWithTag("TransitOverview").transform;
    }

    public override void RunUpdate() {
        cameraController.transform.position = Vector3.Lerp(cameraController.transform.position, birdViewTransform.transform.position, Time.deltaTime * 5);
        cameraController.transform.rotation = Quaternion.Lerp(cameraController.transform.rotation, birdViewTransform.transform.rotation, Time.deltaTime * 5);
        
        if(Input.GetKeyDown(KeyCode.Escape))
            stateMachine.ChangeState<DefaultCameraState>();
    }

}
