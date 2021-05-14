using System;
using System.Collections;
using EventCallbacks;
using UnityEngine;
using UnityEngine.UI;

public class DeathFade : MonoBehaviour {

    [SerializeField] private float fadeSpeed;
    
    private Image imageColor;
    
    private void Awake() {
        imageColor = GetComponent<Image>();
    }
    
    private void OnEnable() {
        EventSystem<PlayerDiedEvent>.RegisterListener(StartFadeOut);
        EventSystem<PlayerReviveEvent>.RegisterListener(StartFadeIn);
    }

    private void OnDisable() {
        EventSystem<PlayerDiedEvent>.UnregisterListener(StartFadeOut);
        EventSystem<PlayerReviveEvent>.UnregisterListener(StartFadeIn);
    }

    private void StartFadeOut(PlayerDiedEvent playerDiedEvent) {
        StartCoroutine(FadeOut(true));
    }

    private void StartFadeIn(PlayerReviveEvent playerDiedEvent) {
        StartCoroutine(FadeOut(false));
    }
    
    private IEnumerator FadeOut(bool fadeOut) {
        
        Color uiColor = imageColor.color;
        float fadeAmount;
        
        if (fadeOut)
        {
            Debug.LogWarning("Fade out");
            while (imageColor.color.a <= 1)
            {
                fadeAmount = uiColor.a + (fadeSpeed * Time.deltaTime);

                uiColor = new Color(uiColor.r, uiColor.g, uiColor.b, fadeAmount);
                imageColor.color = uiColor;
                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("Fade in");
            while (imageColor.color.a >= 0)
            {
                fadeAmount = uiColor.a - (fadeSpeed * Time.deltaTime);

                uiColor = new Color(uiColor.r, uiColor.g, uiColor.b, fadeAmount);
                imageColor.color = uiColor;
                yield return null;
            }

        }
        
    }

}
