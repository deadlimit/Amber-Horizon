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
    
    private void OnEnable() {
        EventSystem<AbilityUsed>.RegisterListener(StartAbilityCooldown);
        EventSystem<InteractTriggerEnterEvent>.RegisterListener(DisplayInteractText);
        EventSystem<InteractTriggerExitEvent>.RegisterListener(ClearUIMessage);
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
    
}
