using UnityEngine;
using EventCallbacks;

public class Gate : MonoBehaviour
{   
    private Animator animator;
    private static readonly int Open = Animator.StringToHash("Open");

    [SerializeField] private ParticleSystem horizontalDust, verticalDust;
    
    public void OnEnable() {
        animator = GetComponent<Animator>();
        EventSystem<UnlockEvent>.RegisterListener(DoorUnlocked);
    }
    private void OnDisable() => EventSystem<UnlockEvent>.UnregisterListener(DoorUnlocked);

    private void DoorUnlocked(UnlockEvent ue)
    {
        animator.SetBool(Open, true);

        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
        
        foreach(ParticleSystem system in systems)
            system.Play();
        
    }

    //Called in animation event
    private void PlayDustEffect() {
        horizontalDust.Stop();
        verticalDust.Stop(); 
        horizontalDust.Play();
        verticalDust.Play();
    }

}
