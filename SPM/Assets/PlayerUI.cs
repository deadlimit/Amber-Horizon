using System;
using System.Collections;
using EventCallbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    [SerializeField] private Image dashCooldownImage;
    [SerializeField] private Image blackholeCooldownImage;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private Image healthBackground, healthBarChipAway, healthBar;
    [SerializeField] private AudioClip UIMessageSFX;
    
    private PlayerController player;
    
    private void Start() {
        EventSystem<AbilityUsed>.RegisterListener(StartAbilityCooldown);
        EventSystem<InteractTriggerEnterEvent>.RegisterListener(DisplayInteractText);
        EventSystem<InteractTriggerExitEvent>.RegisterListener(ClearUIMessage);
        EventSystem<PlayerHitEvent>.RegisterListener(ChangeHealthUI);
        EventSystem<PlayerReviveEvent>.RegisterListener(RestoreHealthUI);
        EventSystem<DisplayUIMessage>.RegisterListener(DisplayMessageOnUI);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        ChangeColor(0);

    }

    private void ChangeColor(float value) {
        
        Color trans = healthBar.color;
        trans.a = value;
        healthBar.color = trans;
        Color trans1 = healthBackground.color;
        trans1.a = value;
        healthBackground.color = trans1;

        Color trans2 = healthBarChipAway.color;
        trans2.a = value;
        healthBarChipAway.color = trans2;
    }

    private void OnDisable() {
        EventSystem<AbilityUsed>.UnregisterListener(StartAbilityCooldown);
        EventSystem<InteractTriggerExitEvent>.UnregisterListener(ClearUIMessage);
        EventSystem<DisplayUIMessage>.UnregisterListener(DisplayMessageOnUI);
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

    private IEnumerator AnimateHealthChipAway(float time, float healthFraction)
    {
        float end = Time.time + time;

        while (Time.time < end && healthBarChipAway.fillAmount > healthFraction) {
            healthBarChipAway.fillAmount -= Time.deltaTime / time;
            yield return null;
        }

        healthBarChipAway.fillAmount = healthFraction;
    }

    private void DisplayInteractText(InteractTriggerEnterEvent trigger) {
        interactText.text = trigger.UIMessage;
    }

    private void DisplayMessageOnUI(DisplayUIMessage message) {
        if(message.PlayUISFX)
            EventSystem<SoundEffectEvent>.FireEvent(new SoundEffectEvent(UIMessageSFX));
        StartCoroutine(SpellOutText(message.UIMessage));
        this.Invoke(() => ClearUIMessage(null), message.duration);
    }

    private IEnumerator SpellOutText(string message) {
        string word = "";
        
        foreach (char letter in message) {
            word += letter;
            interactText.text = word;
            yield return new WaitForSeconds(.02f);
        }
    }
    
    private void ClearUIMessage(InteractTriggerExitEvent exitEvent) {
        interactText.text = "";
    }

    private void ChangeHealthUI(PlayerHitEvent playerHitEvent) {
        float currentHealth = playerHitEvent.player.GetPlayerHealth();

        //4 is the max health. and it doesnt show health bar on death
        if (currentHealth < 1 )
        {
            currentHealth = 4;
        }
        else
        {
            ChangeColor(255);
        }

        float healthFraction = currentHealth / 4;
        Debug.Log("currentHealth: " + currentHealth + " healthFraction: " + healthFraction);

        healthBar.fillAmount = healthFraction;

        if (healthBarChipAway.fillAmount > healthFraction)
        {
            //lerp the healthBarChipAway
            StartCoroutine(AnimateHealthChipAway(1.5f, healthFraction));

        }
        else
        {
            healthBarChipAway.fillAmount = 1.0f;
        }

        healthBackground.Invoke(() => ChangeColor(0), 2.0f);

    }

    private void RestoreHealthUI(PlayerReviveEvent playerReviveEvent) {
        ChangeHealthUI(new PlayerHitEvent(null, null, playerReviveEvent.player));
    }

}
