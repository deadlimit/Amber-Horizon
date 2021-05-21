using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonFader : MonoBehaviour {

    public List<Image> buttons;
    [SerializeField] private float fadeSpeed;
    
    public IEnumerator FadeButtonsSequence(float targetValue) {
        foreach (Image button in buttons) {
            StartCoroutine(FadeButton(button, targetValue));
            yield return new WaitForSeconds(.2f);
        }
    }

    private IEnumerator FadeButton(Image image, float targetValue) {
        float startValue = image.fillAmount;
        float timeElapsed = Time.deltaTime / fadeSpeed;
        
        while (image.fillAmount - targetValue > 0f || image.fillAmount - targetValue < 0f) {
            timeElapsed+= Time.deltaTime / fadeSpeed;
            image.fillAmount = Mathf.Lerp(startValue, targetValue, timeElapsed);
            yield return null;
        }
        
    }

}
