using UnityEngine;

public abstract class InteractableObject : MonoBehaviour {
    
    public string UIMessage;
    
    //Vad som händer när man går in i triggern
    protected abstract void EnterTrigger(string UIMessage);

    //Vad som händer när man befinner sig i triggern
    protected abstract void InsideTrigger(GameObject entity);

    //Vad som händer när man går ur triggern
    protected abstract void ExitTrigger();
    
    
    private void OnTriggerEnter(Collider other) {
        if (PlayerEntered(other.gameObject) == false) return;
        EnterTrigger(UIMessage);
    }
    
    
    private void OnTriggerStay(Collider other) {
        if (PlayerEntered(other.gameObject) == false) return;
        InsideTrigger(other.gameObject);
    }

    
    private void OnTriggerExit(Collider other) {
        if (PlayerEntered(other.gameObject) == false) return;
        ExitTrigger();
    }

    private bool PlayerEntered(GameObject entityEntered) {
        return entityEntered.CompareTag("Player");
    }
}



