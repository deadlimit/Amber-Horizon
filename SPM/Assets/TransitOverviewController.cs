using EventCallbacks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class TransitOverviewController : MonoBehaviour {

    public Button TransitButton;
    
    private void OnEnable() => EventSystem<EnterTransitView>.RegisterListener(TransitView);

    private void OnDisable() => EventSystem<EnterTransitView>.UnregisterListener(TransitView);
    private void TransitView(EnterTransitView view) {

        foreach (Checkpoint checkpoint in Checkpoint.activeCheckpoints) {
            print("IN   ");
            Button button = Instantiate(TransitButton, transform.position, quaternion.identity, gameObject.transform);
            button.GetComponent<RectTransform>().position = Camera.main.WorldToViewportPoint(checkpoint.transform.position);
            
        }
        
    }

}
