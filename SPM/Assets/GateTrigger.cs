using System;
using UnityEngine;

public class GateTrigger : MonoBehaviour {

    [SerializeField] private Animator gateAnimator;
    private static readonly int Open = Animator.StringToHash("Open");
    private BoxCollider trigger;

    private void Awake() {
        trigger = GetComponent<BoxCollider>();
    }
    
    private void OnTriggerEnter(Collider other) {
        gateAnimator.SetBool(Open, true);
        trigger.enabled = false;
    }
}
