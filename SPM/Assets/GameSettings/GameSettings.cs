using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Game Settings", menuName = "Game Settings/Game Settings")]
public class GameSettings : ScriptableObject {


    private float masterVolume;

    public float MasterVolume {

        get => masterVolume;

        set {
            masterVolume = value;

            masterVolume = Mathf.Clamp(masterVolume, 0, 1);

            AudioListener.volume = masterVolume;
        }
    }


}
