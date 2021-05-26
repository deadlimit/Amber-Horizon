using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

    [SerializeField] private Settings.Settings settings;
    
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown resolutionList;
    [SerializeField] private TMP_Dropdown displayModeList;

    private string[] chosenResolution;
        
    private void Awake() {
        volumeSlider.onValueChanged.AddListener(UpdateMaserVolume);
        resolutionList.onValueChanged.AddListener(ChangeResolution);
        displayModeList.onValueChanged.AddListener(ChangeDisplayMode);
        chosenResolution = new string[2];
        
        AutoDetectScreenResolution();
        
    }
    
    private void UpdateMaserVolume(float newValue) {
        settings.MasterVolume = newValue;
    }

    private void ChangeResolution(int index) {

        if (!resolutionList.options[index].text.Contains("x")) {
            AutoDetectScreenResolution();
            return;
        }
        
        chosenResolution = resolutionList.options[index].text.Split('x');
        
        int width = Int32.Parse(chosenResolution[0]);
        int height = Int32.Parse(chosenResolution[1]);

        Settings.Resolution resolution;

        resolution.Width = width;
        resolution.Height = height;

        settings.Resolution = resolution;
    }

    private void ChangeDisplayMode(int index) {
        string choice = displayModeList.options[index].text;
        
        switch (choice) {
            case "Fullscreen":
                Screen.fullScreen = true;
                break;
            case "Windowed":
                Screen.fullScreen = false;
                break;
            
        }
    }

    public void AutoDetectScreenResolution() {
        resolutionList.value = resolutionList.options.FindIndex(resolutionOption => resolutionOption.text == $"{Screen.currentResolution.width}x{Screen.currentResolution.height}");
    }
    
}
