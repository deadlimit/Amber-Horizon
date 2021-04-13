using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DestructorFist : MonoBehaviour {

    private SphereCollider collider;

    public LayerMask PlayerMask;

    public float hitForce;
    
    private void Awake() {
        collider = GetComponent<SphereCollider>();
    }

    public void SwitchCollider(bool value) => collider.enabled = value;
    
    public void Update() {
        Collider[] player = Physics.OverlapSphere(transform.position, collider.radius, PlayerMask);

        if (player.Length < 1) return;
        
        player[0].GetComponent<PhysicsComponent>().AddForce(transform.forward + Vector3.up * hitForce);
        collider.enabled = false;
        enabled = false;
    }
    
}
