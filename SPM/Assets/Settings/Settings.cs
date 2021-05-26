using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Settings/Settings")]
public class Settings : ScriptableObject {
    
    private float masterVolume;
    private Resolution resolution;
    
    public float MasterVolume {
        get => masterVolume;
        set {
            masterVolume = value;
            masterVolume = Mathf.Clamp(masterVolume, 0, 1);
            AudioListener.volume = masterVolume;
        }
    }
    
    public Resolution Resolution {
        get => resolution;
        set {
            resolution = value;
            Debug.Log(resolution.width);
            Debug.Log(resolution.height);
            Screen.SetResolution(resolution.width, resolution.height, FullScreenMode.FullScreenWindow);
        }
    }
}

