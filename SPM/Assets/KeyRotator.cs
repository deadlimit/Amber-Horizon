using System.Threading.Tasks;
using EventCallbacks;
using TMPro;
using UnityEngine;

public class KeyRotator : MonoBehaviour {
    
    private TextMeshPro text;
    private string keyText;
    private bool showing;

    private void Awake() {
        EventSystem<KeyPickUpEvent>.RegisterListener(UpdateKeyText);
        text = GetComponent<TextMeshPro>();
        text.text = string.Empty;
        showing = false;
    }
    
    private void Start() {
        UpdateKeyText(null);

    }

    private void OnDisable()
    {
        EventSystem<KeyPickUpEvent>.UnregisterListener(UpdateKeyText);
    }

    private void UpdateKeyText(KeyPickUpEvent KeyEvent) {
        keyText = GateLock.keysAcquired.Count + "/" + GateLock.keyList.Count + "\n" + "key fragments" + "\n" + "acquired";
    }

    
    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showing = !showing;
            text.text = showing ? keyText : string.Empty;
        }
        
    }


}
