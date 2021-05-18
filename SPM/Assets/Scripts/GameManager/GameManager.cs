using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour {
    
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private Animator cameraAnimator;

    [SerializeField] private MainMenuButtonFader mainMenuButtonFader;
    [SerializeField] private MainMenuButtonFader settingsButtonFader;
    [SerializeField] private Transform settingsPosition;
    [SerializeField] private Transform mainMenuPosition;

    
    private readonly int startGameHash = Animator.StringToHash("OnSceneLoad");
    private readonly int settingsHash = Animator.StringToHash("OnSettingsLoad");

    private readonly int moveToSettingsHash = Animator.StringToHash("MoveToSettings");
    private readonly int movetoMainMenuHash = Animator.StringToHash("MoveToMainMenu");


    private void Start() {
        mainMenuButtonFader.transform.SetAsLastSibling();
        StartCoroutine(mainMenuButtonFader.FadeButtonsSequence(1));
    }
    
    public void OnStartGame(string sceneName) {
        characterAnimator.SetTrigger(startGameHash);
    }
    
    public void OnSettingsPressed() {
        characterAnimator.SetTrigger(settingsHash);
        cameraAnimator.SetTrigger(moveToSettingsHash);
        StartCoroutine(mainMenuButtonFader.FadeButtonsSequence(0));
        this.Invoke(() => { characterAnimator.transform.position = settingsPosition.position; }, 1f);
        
        this.Invoke(() => {
            settingsButtonFader.transform.SetAsLastSibling();
            StartCoroutine(settingsButtonFader.FadeButtonsSequence(1));
        }, 1.2f);
    }

    public void OnBackFromSettingsPressed() {
        characterAnimator.SetTrigger(settingsHash);
        cameraAnimator.SetTrigger(movetoMainMenuHash);
        StartCoroutine(settingsButtonFader.FadeButtonsSequence(0));
        this.Invoke( () => characterAnimator.transform.position = mainMenuPosition.position, 1f);
        
        this.Invoke(() => {
            mainMenuButtonFader.transform.SetAsLastSibling();
            StartCoroutine(mainMenuButtonFader.FadeButtonsSequence(1));
        }, 1.2f);
        
    }
    
    
    
}
