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
        
        chosenResolution = resolutionList.options[index].text.Split('x');
        
        int width = int.Parse(chosenResolution[0]);
        int height = int.Parse(chosenResolution[1]);

        Settings.Resolution resolution;

        resolution.Width = width;
        resolution.Height = height;

        settings.Resolution = resolution;
        
    }

    private void ChangeDisplayMode(int index) {
        string choice = displayModeList.options[index].text;

        Screen.fullScreen = choice switch {
            "Fullscreen" => true,
            "Windowed" => false,
            _ => Screen.fullScreen
        };
    }

    private void AutoDetectScreenResolution() {
        resolutionList.value = resolutionList.options.FindIndex(resolutionOption => resolutionOption.text.Equals($"{Screen.currentResolution.width}x{Screen.currentResolution.height}"));
        ChangeResolution(resolutionList.value);
    }

    public static void Quit() {
        Debug.Log("In SettingsController. Pressed Quit.");
        Application.Quit();
    }
    
}
