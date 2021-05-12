using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathFade : MonoBehaviour
{

    private GameObject ui;

    void Start()
    {
        ui = this.gameObject;
    }

    public IEnumerator FadeOut(bool fadeOut = true, int fadeSpeed = 1)
    {
        Color uiColor = ui.GetComponent<Image>().color;
        float fadeAmount;
        if (fadeOut)
        {
            while (ui.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = uiColor.a + (fadeSpeed * Time.deltaTime);

                uiColor = new Color(uiColor.r, uiColor.g, uiColor.b, fadeAmount);
                ui.GetComponent<Image>().color = uiColor;
                yield return null;
            }
        }
        else
        {
            while (ui.GetComponent<Image>().color.a > 0)
            {
                fadeAmount = uiColor.a - (fadeSpeed * Time.deltaTime);

                uiColor = new Color(uiColor.r, uiColor.g, uiColor.b, fadeAmount);
                ui.GetComponent<Image>().color = uiColor;
                yield return null;
            }

        }
    }

}
