using EventCallbacks;
using UnityEngine;

public class KeyRotator : MonoBehaviour {

    public float RotationSpeed;
    private MeshRenderer text;
    private Transform thisTransform;
    private void Awake() {
        thisTransform = GetComponent<Transform>();
        text = GetComponent<MeshRenderer>();
        text.enabled = false;
    }

    
    private void Update() {
        
        if(Input.GetKeyDown(KeyCode.Tab))
            text.enabled = !text.enabled;
        
        thisTransform.rotation *= Quaternion.Euler(0, 10 * RotationSpeed * Time.deltaTime, 0);
    }


}
