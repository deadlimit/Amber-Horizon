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
        EventSystem<NewLevelLoadedEvent>.RegisterListener(ResetKeyText);
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
        EventSystem<NewLevelLoadedEvent>.RegisterListener(ResetKeyText);
    }

    private void UpdateKeyText(KeyPickUpEvent keyEvent) {
        keyText = GateLock.KeysAcquired.Count + "/" + GateLock.KeyList.Count + "\n" + "key fragments" + "\n" + "acquired";
    }

    private void ResetKeyText(NewLevelLoadedEvent newLevelLoadedEvent) {
        UpdateKeyText(null);
    }
    
    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showing = !showing;
            text.text = showing ? keyText : string.Empty;
        }
        
    }


}
