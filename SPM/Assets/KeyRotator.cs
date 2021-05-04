using EventCallbacks;
using UnityEngine;

public class KeyRotator : MonoBehaviour {

    //public float RotationSpeed;
    private TextMesh text;
    private Transform thisTransform;
    private string keyText;
    private bool showing;

    private void Awake() {
        EventSystem<KeyPickUpEvent>.RegisterListener(UpdateKeyText);
        thisTransform = GetComponent<Transform>();
        text = GetComponent<TextMesh>();
        text.text = string.Empty;
        showing = false;
    }

    private void Start()
    {
        keyText = GateLock.keysAcquired.Count + "/" + GateLock.keyList.Count;

    }

    private void OnDisable()
    {
        EventSystem<KeyPickUpEvent>.UnregisterListener(UpdateKeyText);
    }

    private void UpdateKeyText(KeyPickUpEvent KeyEvent)
    {
        keyText = GateLock.keysAcquired.Count + "/" + GateLock.keyList.Count;
    }

    
    private void Update() {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showing = !showing;
            text.text = showing ? keyText : string.Empty;
        }
            
        
     
    }


}
