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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            StartCoroutine(FadeOut());
        if (Input.GetKeyDown(KeyCode.J))
            StartCoroutine(FadeOut(false));

    }

    public IEnumerator FadeOut(bool fadeOut = true, int fadeSpeed = 5)
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
