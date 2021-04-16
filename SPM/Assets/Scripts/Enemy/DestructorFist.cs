using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class DestructorFist : MonoBehaviour {

    private SphereCollider coll;

    public LayerMask PlayerMask;

    public float hitForce;
    
    private void Awake() {
        coll = GetComponent<SphereCollider>();
    }

    public void SwitchCollider(bool value) => coll.enabled = value;
    
    public void Update() {
        Collider[] player = Physics.OverlapSphere(transform.position, coll.radius, PlayerMask);

        if (player.Length < 1) return;
        
        player[0].GetComponent<PhysicsComponent>().AddForce(transform.forward + Vector3.up * hitForce);
        coll.enabled = false;
        enabled = false;
    }
    
}
