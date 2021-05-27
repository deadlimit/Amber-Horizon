using UnityEngine;

public class TunnelEndGate : MonoBehaviour {

    [SerializeField] private Animator animator;

    private BoxCollider trigger;

    private void Awake() => trigger = GetComponent<BoxCollider>();
    
    private static readonly int OpenGateHash = Animator.StringToHash("OpenGate");
    private static readonly int CloseGateHash = Animator.StringToHash("CloseGate");
    
    private void OnTriggerEnter(Collider other) {
        print("trigger to level 2");
        animator.SetTrigger(OpenGateHash);
        trigger.enabled = false;
        
    }

}
