using EventCallbacks;
using UnityEngine;

public class SceneTransit : MonoBehaviour {

    public string NewSceneName;
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") == false)
            return;

        EventSystem<StartSceneTransitEvent>.FireEvent(new StartSceneTransitEvent(NewSceneName));
        Destroy(gameObject);
    }
}
