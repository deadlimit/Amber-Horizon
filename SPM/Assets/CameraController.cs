using System;
using Cinemachine;
using EventCallbacks;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour {

    private bool canMove;
    private Animator animator;
    private PlayerController playerController;
    private ThirdPersonCamera cameraController;
    [SerializeField] private CinemachineVirtualCamera playerCamera, keyLookAtCamera;
    
    private static readonly int ShowKey = Animator.StringToHash("ShowKey");

    private void OnEnable() {
        EventSystem<PlayerReviveEvent>.RegisterListener(OnRespawn);
    }

    private void OnDisable() {
        EventSystem<PlayerReviveEvent>.UnregisterListener(OnRespawn);
    }

    private void Awake() {
        canMove = false;
        playerController = FindObjectOfType<PlayerController>();
        cameraController = FindObjectOfType<ThirdPersonCamera>();
        animator = playerController.GetComponent<Animator>();
    }
    public void Update() {
        
        if (Input.GetKeyDown(KeyCode.Tab)) {
            ToggleLockedState(canMove);
        }
    }

    private void ToggleLockedState(bool value) {
        
        EventSystem<ShowKeyText>.FireEvent(new ShowKeyText(!value));
        
        animator.SetBool("ShowKey", !value);
        playerController.enabled = value;
        cameraController.enabled = value;
        canMove = !canMove;
    }

    private void OnRespawn(PlayerReviveEvent reviveEvent) {
        animator.SetBool("ShowKey", false);
        playerController.enabled = true;
        cameraController.enabled = true;
        EventSystem<ShowKeyText>.FireEvent(new ShowKeyText(false));
    }
    

}
