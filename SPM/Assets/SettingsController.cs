using System;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour {

    [SerializeField] private Settings settings;
    
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown resolutionList;

    private string[] chosenResolution;
        
    private void Awake() {
        volumeSlider.onValueChanged.AddListener(UpdateMaserVolume);
        resolutionList.onValueChanged.AddListener(ChangeResolution);
        chosenResolution = new string[2];
    }
    
    private void UpdateMaserVolume(float newValue) {
        settings.MasterVolume = newValue;
    }

    private void ChangeResolution(int index) {
        chosenResolution = resolutionList.options[index].text.Split('x');

        int width = Int32.Parse(chosenResolution[0]);
        int height = Int32.Parse(chosenResolution[1]);

        Resolution resolution = new Resolution();

        resolution.width = width;
        resolution.height = height;

        settings.Resolution = resolution;
    }
    
}
