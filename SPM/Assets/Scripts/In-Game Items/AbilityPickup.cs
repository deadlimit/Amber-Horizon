using AbilitySystem;
using EventCallbacks;
using UnityEngine;

public class AbilityPickup : MonoBehaviour {

    [SerializeField] private GameplayAbility ability;

    public void OnTriggerEnter(Collider other) {
        
        if (!other.CompareTag("Player")) return;
        
        other.GetComponent<GameplayAbilitySystem>().GrantAbility(ability);
        EventSystem<DisplayUIMessage>.FireEvent(new DisplayUIMessage("New ability: Dash.\n Press shift to use.", 5, true));
        Destroy(gameObject);
    }
}
