using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    
    [SerializeField] private GameSettings GameSettings;
    
    [SerializeField] private Slider volumeSlider;
    
    private void OnEnable() {
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void OnDisable() {
        volumeSlider.onValueChanged.RemoveAllListeners();
    }

    private void ChangeVolume(float newValue) {
        GameSettings.MasterVolume = newValue;
    }
    
}
