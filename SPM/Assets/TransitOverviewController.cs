using System.Collections;
using System.Collections.Generic;
using EventCallbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TransitOverviewController : MonoBehaviour {

    public GameObject TransitButton;
    public float WaitUntilButtonSpawn;
    private List<GameObject> activeButtons = new List<GameObject>();
    
    private void OnEnable() {
        EventSystem<EnterTransitView>.RegisterListener(TransitView);
        EventSystem<ExitTransitView>.RegisterListener(ExitView);
    }

    private void OnDisable() {
        EventSystem<EnterTransitView>.UnregisterListener(TransitView);
        EventSystem<ExitTransitView>.UnregisterListener(ExitView);
    }
    
    
    private void TransitView(EnterTransitView view) {
        StartCoroutine(SpawnButtons(view.TransitUnits, view.ActivatedTransitUnit));
    }

    private IEnumerator SpawnButtons(HashSet<TransitUnit> buttons, TransitUnit activatedTransitUnit) {

        yield return new WaitForSeconds(WaitUntilButtonSpawn);

        foreach (TransitUnit transitUnit in buttons) {
            GameObject button = Instantiate(TransitButton, Camera.main.WorldToScreenPoint(transitUnit.transform.position), Quaternion.identity, gameObject.transform);
            
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            if (transitUnit == activatedTransitUnit) {
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
        EventSystem<ExitTransitView>.FireEvent(null);
    }

    private void ExitView(ExitTransitView view) {
        StopCoroutine(SpawnButtons(null, null));
        foreach (GameObject button in activeButtons)
            Destroy(button.gameObject);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }
    
    

}
