using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class PlayerHitListener : MonoBehaviour
{
    AbilitySystem.GameplayAbilitySystem gas;
    private void Start()
    {
        EventSystem<PlayerHitEvent>.RegisterListener(OnPlayerHit);
        gas = GetComponent<AbilitySystem.GameplayAbilitySystem>();
        Debug.Assert(gas);
    }
    private void OnDisable() => EventSystem<PlayerHitEvent>.UnregisterListener(OnPlayerHit);

    private void OnPlayerHit(PlayerHitEvent phe)
    {
       
        Debug.Log("OnPlayerHitEvent");
        if(gas.GetAttributeValue(typeof(HealthAttribute)) <= 0)
        {
            PlayerDiedEvent pde = new PlayerDiedEvent();
            EventSystem<PlayerDiedEvent>.FireEvent(pde);
        }
    }
}
