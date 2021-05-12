using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class PowderKeg : MonoBehaviour
{
    public GameObject explosionVFX;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EventSystem<ExplosionEvent>.FireEvent(new ExplosionEvent(transform.position));
            Instantiate(explosionVFX, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
