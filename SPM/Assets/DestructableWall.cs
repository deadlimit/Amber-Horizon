using System.Collections.Generic;
using UnityEngine;

public class DestructableWall : MonoBehaviour {

    private List<Rigidbody> children = new List<Rigidbody>();

    private void Awake() {
        for(int i = 0; i < transform.childCount; i++)
            children.Add(transform.GetChild(i).GetComponent<Rigidbody>());
    }
    

}
