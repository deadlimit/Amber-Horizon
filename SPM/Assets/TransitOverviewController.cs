using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitOverviewController : MonoBehaviour {

    [SerializeField] private GameObject transitButton;
    [SerializeField] private float waitUntilButtonSpawn;
    private readonly List<GameObject> activeButtons = new List<GameObject>();

    [SerializeField] private Canvas canvas;
    
    private Coroutine spawnButtons;
    private RectTransform canvasRect;
    private void OnEnable() {
        EventSystem<EnterTransitViewEvent>.RegisterListener(TransitView);
        EventSystem<ResetCameraFocus>.RegisterListener(ExitView);
        canvasRect = canvas.GetComponent<RectTransform>();
    }

    private void OnDisable() {
        EventSystem<EnterTransitViewEvent>.UnregisterListener(TransitView);
        EventSystem<ResetCameraFocus>.UnregisterListener(ExitView);
    }
    
    private void TransitView(EnterTransitViewEvent viewEvent) {
        spawnButtons = StartCoroutine(SpawnButtons(viewEvent));
    }

    private IEnumerator SpawnButtons(EnterTransitViewEvent viewEvent) {
        
        yield return new WaitForSeconds(waitUntilButtonSpawn);
        
        foreach (TransitUnit transitUnit in viewEvent.TransitUnits) {
            GameObject button = Instantiate(transitButton, Camera.main.WorldToScreenPoint(transitUnit.transform.position), Quaternion.identity, canvas.transform);
            
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            if (transitUnit == viewEvent.ActivatedTransitUnit) {
                buttonText.text = "You are here";
                buttonText.GetComponentInParent<Image>().color = Color.green;
            }
                
            else {
                buttonText.text = "Travel here";
                button.GetComponent<Button>().onClick.AddListener(() => MovePlayer(transitUnit));
            }
            
            activeButtons.Add(button);
        }
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void MovePlayer(TransitUnit transitUnit) {
        GameObject.FindGameObjectWithTag("Player").transform.position = transitUnit.AttachedCheckpoint.SpawnPosition;
        EventSystem<ResetCameraFocus>.FireEvent(null);
    }

    private void ExitView(ResetCameraFocus viewEvent) {
        StopCoroutine(spawnButtons);
        foreach (GameObject button in activeButtons)
            Destroy(button.gameObject);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



}
