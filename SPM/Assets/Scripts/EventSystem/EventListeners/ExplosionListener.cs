using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
public class ExplosionListener : MonoBehaviour
{
    public GameObject explosion;
    private void OnEnable() => EventSystem<ExplosionEvent>.RegisterListener(Explosion);

    private void Explosion (ExplosionEvent ee)
    {
        Instantiate(explosion, ee.location, Quaternion.identity);

    }
}
