using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuController : MonoBehaviour {

    [SerializeField] private List<Button> buttons;

    [SerializeField] private Animator animator;
    
    private static readonly int OpenMenuHash = Animator.StringToHash("OpenMenu");
    private static readonly int CloseMenu = Animator.StringToHash("CloseMenu");

    private bool menuActive;
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            animator.SetTrigger(menuActive ? CloseMenu : OpenMenuHash);
            
            menuActive = !menuActive;
        }
    }
    
    //Called in animation event
    public void ActivateMenu() {
        SetInteractiveButtons(true);
        Cursor.ActivateCursor(true, CursorLockMode.Confined);
        EventSystem<ActivatePlayerControl>.FireEvent(new ActivatePlayerControl(false));
    }
    
    //Called in animation event
    public void DeactivateMenu() {
        SetInteractiveButtons(false);
        Cursor.ActivateCursor(false, CursorLockMode.Locked);
        EventSystem<ActivatePlayerControl>.FireEvent(new ActivatePlayerControl(true));
    }

    private void SetInteractiveButtons(bool value) {
        foreach (Button button in buttons)
            button.interactable = value;
    }

    public void Resume() {
        animator.SetTrigger(CloseMenu);
        menuActive = !menuActive;
    }
    
    public void QuitGame() {
        Application.Quit();
    }
    
}
