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

        //Istället för att lägga in abilitySystem på varje checkpoint aktiverar jag en metod i spelaren härifrån
        // inte supercleant men det är här kollisionen kollas, och känns så orimligt att bygga upp det från checkpointens håll
        if (unvisited)
        {
            unvisited = false;
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            pc.RestoreHealth();
        }

    }
}
