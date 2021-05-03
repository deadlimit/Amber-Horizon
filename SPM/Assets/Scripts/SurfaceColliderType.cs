using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceColliderType : MonoBehaviour
{
    public enum Mode { Default, House, Plattform } // att skapa olika val för marken
    public Mode terrainType;

    public string GetTerrainType()
    {
        string typeString = "";

        switch (terrainType) // Att switcha olika mode
        {
            case Mode.Default:
                typeString = "Default";
                break;
            case Mode.House:
                typeString = "House";
                break;
            case Mode.Plattform:
                typeString = "Plattform";
                break;
            default:
                typeString = "";
                break;

        }

        return typeString;
    }
}
