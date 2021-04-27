using System.Collections.Generic;
using UnityEngine;

public class DestructableWall : MonoBehaviour, IBlackHoleBehaviour {

    private List<Transform> children = new List<Transform>();

    public LayerMask collisionMask;
    
    private void Awake() {
        for(int i = 0; i < transform.childCount; i++)
            children.Add(transform.GetChild(i).GetComponent<Transform>());
    }


    public void BlackHoleBehaviour(BlackHole blackHole) {
        transform.DetachChildren();
        Destroy(gameObject);
        foreach (Transform child in children) {
            child.gameObject.AddComponent<Rigidbody>();
            PhysicsComponent physics = child.gameObject.AddComponent<PhysicsComponent>();
            physics.collisionMask = collisionMask;
            physics.gravity = 0;
            //print("wall");
        }
        Debug.Log("Efter loop");


    }
}
