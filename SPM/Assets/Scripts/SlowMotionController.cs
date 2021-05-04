using System.Collections;
using EventCallbacks;
using UnityEngine;

public class SlowMotionController : MonoBehaviour {

    [Range(0, 1)]
    public float SlowMotionResetSpeed;
    
    private void OnEnable() {
        EventSystem<EnterSlowMotionEvent>.RegisterListener(EnterSlowMotion);
    }

    private void OnDisable() {
        EventSystem<EnterSlowMotionEvent>.UnregisterListener(EnterSlowMotion);
    }
    
    private void EnterSlowMotion(EnterSlowMotionEvent slowMotionEvent) {
        Time.timeScale = .05f;
        Time.fixedDeltaTime = Time.timeScale * .2f;
        StartCoroutine(ResetTime(slowMotionEvent.duration));
    }

    private IEnumerator ResetTime(float timeBeforeReset) {

        yield return new WaitForSeconds(timeBeforeReset / 100);
        
        
        while (Time.timeScale < 1) {
            Time.timeScale += SlowMotionResetSpeed * Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);


    }
    
}
