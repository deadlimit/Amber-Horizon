using UnityEngine;

public class Cursor {

    public static void ActivateCursor(bool shouldBeVisible, CursorLockMode cursorLockMode) {
        UnityEngine.Cursor.lockState = cursorLockMode;
        UnityEngine.Cursor.visible = shouldBeVisible;
    } 
    


}
