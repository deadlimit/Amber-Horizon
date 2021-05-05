using System;
using System.Collections.Generic;
using EventCallbacks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventPanel : InteractableObject {
    
    [SerializeField] private GameObject eventObject;
    [SerializeField] private List<GameObject> eventObjects;

    private Image percentBar;
    private IEventPanelInteract eventInteraction;
    private List<IEventPanelInteract> eventInteractions;
    private TextMeshProUGUI percentText;
    private Action activateFunction;
    private Action barFillDone;
    
    private string UIText;
    
    private void Awake() {
        eventInteractions = new List<IEventPanelInteract>();
        percentBar = GetComponentInChildren<Image>();
        percentText = GetComponentInChildren<TextMeshProUGUI>();
        
        if (eventObjects.Count < 1) {
            eventInteraction = eventObject.GetComponent<IEventPanelInteract>();
            activateFunction = ExecuteFromSingleObject;
            barFillDone = EventDone;
        }

        else {
            foreach (GameObject eventObject in eventObjects)
                eventInteractions.Add(eventObject.GetComponent<IEventPanelInteract>());
            
            activateFunction = ExecuteFromList;
            barFillDone = EventsDone;
        }
            
        UIText = GeneratePercentText(percentBar.fillAmount);
    }
    
    protected override void EnterTrigger(string UIMessage) {
        if(activateFunction != null)
            EventSystem<InteractTriggerEnterEvent>.FireEvent(new InteractTriggerEnterEvent(UIMessage));
        percentBar.enabled = true;
        percentText.text = UIText;
    }

    protected override void InsideTrigger(GameObject entity) {

        if (activateFunction is null)
            return;
        
        if (Input.GetKey(KeyCode.F))
            activateFunction();
        
    }

    protected override void ExitTrigger() {
        EventSystem<InteractTriggerExitEvent>.FireEvent(null);
        percentText.text = string.Empty;
        percentBar.enabled = false;
    }

    private void ExecuteFromList() {
        float lowestTotalPercentageDone = 100;

        foreach (IEventPanelInteract eventInteraction in eventInteractions) {

            float percentageDone = eventInteraction.CalculatePercentageDone();

            if (percentageDone < lowestTotalPercentageDone)
                lowestTotalPercentageDone = percentageDone;
            
            eventInteraction.ActivateEvent();
        }

        SetUITexts(lowestTotalPercentageDone);
    }

    private void SetUITexts(float newPercentBarValue) {
        
        percentBar.fillAmount = newPercentBarValue;
        UIText = percentText.text = percentBar.fillAmount < 0.99f ? GeneratePercentText(percentBar.fillAmount) : "Completed";

        if (percentBar.fillAmount > .99f) {
            barFillDone();
            activateFunction = null;
            EventSystem<InteractTriggerExitEvent>.FireEvent(null);
        }
            
    }
    
    private void ExecuteFromSingleObject() {
        eventInteraction?.ActivateEvent();
        SetUITexts(eventInteraction.CalculatePercentageDone());
    }

    private string GeneratePercentText(float valueToConvert) {
        return (int)(valueToConvert * 100) + " %";
    }

    private void EventDone() {
        eventInteraction.EventDone();
    }

    private void EventsDone() {
        foreach(IEventPanelInteract eventInteraction in eventInteractions)
            eventInteraction.EventDone();
    }
    
}
