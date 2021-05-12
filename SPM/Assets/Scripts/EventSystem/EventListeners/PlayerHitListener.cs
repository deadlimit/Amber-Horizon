using UnityEngine;
using EventCallbacks;

public class PlayerHitListener : MonoBehaviour
{
    AbilitySystem.GameplayAbilitySystem gas;

    private bool isVurnable;
    
    private void Start()
    {
        EventSystem<PlayerHitEvent>.RegisterListener(OnPlayerHit);
        gas = GetComponent<AbilitySystem.GameplayAbilitySystem>();
        Debug.Assert(gas);
        isVurnable = true;
    }
    private void OnDisable() => EventSystem<PlayerHitEvent>.UnregisterListener(OnPlayerHit);

    private void OnPlayerHit(PlayerHitEvent phe) {
        if (isVurnable == false)
            return;
        
        gas.ApplyEffectToSelf(phe.appliedEffect);
        
        if(gas.GetAttributeValue(typeof(HealthAttribute)) <= 0)
        {
            PlayerDiedEvent pde = new PlayerDiedEvent(gameObject);
            isVurnable = false;
            this.Invoke(() => isVurnable = true, 2);
            EventSystem<PlayerDiedEvent>.FireEvent(pde);
        }
    }
}
