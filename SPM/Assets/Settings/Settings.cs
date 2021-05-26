using System;
using UnityEngine;

namespace Settings {

    [CreateAssetMenu(fileName = "Settings", menuName = "Settings/Settings")]
    public class Settings : ScriptableObject {

        private float masterVolume;
        private Resolution resolution;
        public bool Fullscreen { get; set; } = true;
        
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
                Screen.SetResolution(resolution.Width, resolution.Height, true);
            }
        }

        
    }
    
}