using UnityEngine;

public class FocusCamera : CameraBehaviour {
    
    [SerializeField] private float cameraSpeed;
    
    public override void MovementBehaviour() {
        CameraTransform.transform.position = Vector3.Lerp(CameraTransform.transform.position, Target.transform.position, Time.deltaTime * cameraSpeed);
        CameraTransform.transform.rotation = Quaternion.Lerp(CameraTransform.transform.rotation, Target.transform.rotation, Time.deltaTime * cameraSpeed);
    }
    
}
