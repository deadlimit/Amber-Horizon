using UnityEngine;
using EventCallbacks;

public class Gate : MonoBehaviour
{   
    Animator animator;
    private void OnEnable() 
    {
        animator = GetComponent<Animator>();
    }
    public void OpenGate() {
        animator.SetTrigger("OpenGate");

        ParticleSystem[] systems = GetComponentsInChildren<ParticleSystem>();
        
        foreach(ParticleSystem system in systems)
            system.Play();
        
    }


}
