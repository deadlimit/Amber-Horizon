using System;
using EventCallbacks;
using UnityEngine;

[Serializable]
public class Checkpoint : MonoBehaviour {

    public AudioClip audio;
    public int ID { get; set; }
    private bool unvisited = true;

    public Vector3 SpawnPosition { get; set; }
    
    public Action<int> OnPlayerEnter;

    private void Awake() {
        SpawnPosition = transform.GetChild(0).transform.position;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) return;
            
        OnPlayerEnter?.Invoke(ID);
        EventSystem<CheckPointActivatedEvent>.FireEvent(new CheckPointActivatedEvent(audio));

        //Ist�llet f�r att l�gga in abilitySystem p� varje checkpoint aktiverar jag en metod i spelaren h�rifr�n
        // inte supercleant men det �r h�r kollisionen kollas, och k�nns s� orimligt att bygga upp det fr�n checkpointens h�ll
        if (unvisited)
        {
            unvisited = false;
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            pc.RestoreHealth();
        }

    }
}
