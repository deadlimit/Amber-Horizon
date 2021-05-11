using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCursor : MonoBehaviour
{
    private static AimCursor cursorObject;
    public static AimCursor CursorObject
    {
        get
        {
            if (cursorObject == null)
            {
                cursorObject = FindObjectOfType<AimCursor>();
            }
            return cursorObject;
        }
    }
}
