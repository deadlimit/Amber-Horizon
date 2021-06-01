using UnityEngine;
using EventCallbacks;

public class Gate : MonoBehaviour {
    
    public int ID;
    
    private Animator animator;
    
    private static readonly int Open = Animator.StringToHash("Open");
    private static readonly int Close = Animator.StringToHash("Close");
    
    [SerializeField] private ParticleSystem horizontalDust, verticalDust;

    private AudioSource audioSource;
    
    public void OnEnable() {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        EventSystem<UnlockEvent>.RegisterListener(DoorUnlocked);
    }
    private void OnDisable() => EventSystem<UnlockEvent>.UnregisterListener(DoorUnlocked);

    private void DoorUnlocked(UnlockEvent unlockEvent) {
        if (unlockEvent.ID != ID) return;
        
        animator.SetTrigger(Open);
        PlayDustEffect();
        
    }

    //Called in animation event
    private void PlayDustEffect() {
        horizontalDust.Stop();
        verticalDust.Stop(); 
        horizontalDust.Play();
        verticalDust.Play();
    }

    private void PlaySFX(AudioClip sfx) {
        if(audioSource.isPlaying)
            audioSource.Stop();
        
        audioSource.PlayOneShot(sfx);
    }
    
}
