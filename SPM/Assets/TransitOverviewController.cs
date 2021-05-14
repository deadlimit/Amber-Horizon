using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitOverviewController : MonoBehaviour {

    [SerializeField] private GameObject transitButton;
    [SerializeField] private float waitUntilButtonSpawn;
    [SerializeField] private Canvas UI;
    [SerializeField] private TextMeshProUGUI ExitInstructionText;
    private readonly List<GameObject> activeButtons = new List<GameObject>();
    
    private void OnEnable() {
        EventSystem<EnterTransitViewEvent>.RegisterListener(TransitView);
        EventSystem<ResetCameraFocus>.RegisterListener(ExitView);
        ExitInstructionText.gameObject.SetActive(false);
    }

    private void OnDisable() {
        EventSystem<EnterTransitViewEvent>.UnregisterListener(TransitView);
        EventSystem<ResetCameraFocus>.UnregisterListener(ExitView);
    }
    
    private void TransitView(EnterTransitViewEvent viewEvent) {
        StartCoroutine(SpawnButtons(viewEvent.TransitCameraFocusInfo));
    }

    private IEnumerator SpawnButtons(TransitCameraFocusInfo focusInfo) {
        
        yield return new WaitForSeconds(waitUntilButtonSpawn);
        ExitInstructionText.gameObject.SetActive(true);
        foreach (TransitUnit transitUnit in focusInfo.TransitUnits) {
            GameObject button = Instantiate(transitButton, Camera.main.WorldToScreenPoint(transitUnit.transform.position), Quaternion.identity, UI.transform);
            
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            if (transitUnit == focusInfo.ActivatedTransitUnit) {
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
        StopAllCoroutines();
        foreach (GameObject button in activeButtons)
            Destroy(button.gameObject);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ExitInstructionText.gameObject.SetActive(false);
    }
    
    

}
