using UnityEngine;
using EventCallbacks;

public class PlayerHitListener : MonoBehaviour
{
    AbilitySystem.GameplayAbilitySystem gas;

    private bool isVulnerable;
    
    private void Start()
    {
        EventSystem<PlayerHitEvent>.RegisterListener(OnPlayerHit);
        gas = GetComponent<AbilitySystem.GameplayAbilitySystem>();
        Debug.Assert(gas);
        isVulnerable = true;
    }
    private void OnDisable() => EventSystem<PlayerHitEvent>.UnregisterListener(OnPlayerHit);

    private void OnPlayerHit(PlayerHitEvent phe) {
        if (isVulnerable == false)
            return;

        StartHitAnimationEvent shae = new StartHitAnimationEvent(phe.appliedEffect, phe.culprit);
        EventSystem<StartHitAnimationEvent>.FireEvent(shae);
        isVulnerable = false;
        gas.ApplyEffectToSelf(phe.appliedEffect);
        
        //if player died from this attack
        if(gas.GetAttributeValue(typeof(HealthAttribute)) <= 0)
        {
            PlayerDiedEvent pde = new PlayerDiedEvent(gameObject);
            EventSystem<PlayerDiedEvent>.FireEvent(pde);
        }
    }

    //Called by animation event, which one?? 
    public void SetPlayerVulnerable()
    {
        isVulnerable = true;
    }
}
