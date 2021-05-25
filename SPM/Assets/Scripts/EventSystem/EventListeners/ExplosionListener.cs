using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
public class ExplosionListener : MonoBehaviour
{
    private void OnEnable() => EventSystem<ExplosionEvent>.RegisterListener(Explosion);
    private void OnDisable() => EventSystem<ExplosionEvent>.UnregisterListener(Explosion);


    //vill ju förstås använda object pooling här va
    private void Explosion (ExplosionEvent ee)
    {
        ObjectPooler.Instance.Spawn("Explosion", ee.location, Quaternion.identity);
    }
}
