
using EventCallbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneTransitEffect : MonoBehaviour {

    private Animation transitAnimation;
    private string newSceneName;
    private void OnEnable() {
        EventSystem<StartSceneTransitEvent>.RegisterListener(PlayTransitAnimation);
        transitAnimation = GetComponent<Animation>();
    }
    
    private void OnDisable() => EventSystem<StartSceneTransitEvent>.UnregisterListener(PlayTransitAnimation);

    private void PlayTransitAnimation(StartSceneTransitEvent eventInfo) {
        newSceneName = eventInfo.Scene;
        transitAnimation.Play();
    }

    private void FireNewSceneTransition() {
        EventSystem<SceneTransitEvent>.FireEvent(new SceneTransitEvent(newSceneName));
    }
    


}
