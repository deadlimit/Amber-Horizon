using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPoint : MonoBehaviour
{
    private static LaunchPoint point;
    public static LaunchPoint Point
    {
        get
        {
            if (point == null)
            {
                point = FindObjectOfType<LaunchPoint>();
            }
            return point;
        }
    }
}
