using System;
using AbilitySystem;
using UnityEngine;

public class AbilityPickup : MonoBehaviour {

    public GameplayAbility ability;

    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.GetComponent<GameplayAbilitySystem>().GrantAbility(ability);
            Destroy(gameObject);
        }
    }
}
