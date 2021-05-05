using System;
using System.Collections;
using EventCallbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Image dashCooldownImage;
    public Image blackholeCooldownImage;
    public TextMeshProUGUI interactText;

    private Slider healthSlider;
    private PlayerController pc;

    public Image sliderBackground, sliderFill;
    
    private void Start() {
        EventSystem<AbilityUsed>.RegisterListener(StartAbilityCooldown);
        EventSystem<InteractTriggerEnterEvent>.RegisterListener(DisplayInteractText);
        EventSystem<InteractTriggerExitEvent>.RegisterListener(ClearUIMessage);
        EventSystem<PlayerHitEvent>.RegisterListener(ChangeHealthUI);
        EventSystem<CheckPointActivatedEvent>.RegisterListener(RestoreHealthUI);

        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        healthSlider = GetComponentInChildren<Slider>();

        ChangeColor(0);

    }

    private void ChangeColor(float value) {
        
        Color trans = sliderFill.color;
        trans.a = value;
        sliderFill.color = trans;
        Color trans1 = sliderBackground.color;
        trans1.a = value;
        sliderBackground.color = trans1;
        
    }
    private void OnDisable() {
        EventSystem<AbilityUsed>.UnregisterListener(StartAbilityCooldown);
        EventSystem<InteractTriggerExitEvent>.UnregisterListener(ClearUIMessage);
    }

    private void StartAbilityCooldown(AbilityUsed abilityUsed) {
        if (abilityUsed.ability is DashAbility)
            StartCoroutine(StartAnimation(dashCooldownImage, abilityUsed.ability.Cooldown.Duration));
        if (abilityUsed.ability is BlackHoleAbility)
            StartCoroutine(StartAnimation(blackholeCooldownImage, abilityUsed.ability.Cooldown.Duration));
    }

    private IEnumerator StartAnimation(Image image, float time) {

        float end = Time.time + time;
        
        while (Time.time < end) {
            image.fillAmount += Time.deltaTime / time;
            yield return null;
        }
        
        image.fillAmount = 0;
    }

    private void DisplayInteractText(InteractTriggerEnterEvent trigger) {
        interactText.text = trigger.UIMessage;
    }
    
    private void ClearUIMessage(InteractTriggerExitEvent exitEvent) {
        interactText.text = "";
    }

    private void ChangeHealthUI(PlayerHitEvent playerHitEvent) {
        sliderBackground = GameObject.FindGameObjectWithTag("BackgroundTag").GetComponent<Image>();
        sliderFill = GameObject.FindGameObjectWithTag("FillTag").GetComponent<Image>();
        ChangeColor(255);

        float currentHealth = pc.GetPlayerHealth();
        Debug.Log("reached ChangeHealthUI");
        if(currentHealth < 1 )
        {
            currentHealth = 4;
        }
        healthSlider.value = currentHealth;

        sliderBackground.Invoke(() => ChangeColor(0), 1.5f);
        
    }

    private void RestoreHealthUI(CheckPointActivatedEvent checkPointActivatedEvent)
    {
        ChangeHealthUI(null);
    }

}
