using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionInstance : MonoBehaviour
{
    public SphereCollider coll;
    private float blastArea = 10;
    private void OnEnable()
    {
        Debug.Log("Boom");
        Debug.Assert(coll);
        StartCoroutine(ExpandRadius(coll.radius));
    }

    private IEnumerator ExpandRadius(float startingRadius)
    {
        Debug.Log(startingRadius < blastArea);
        while (coll.radius < 8f)
        {
            coll.radius = Mathf.Lerp(coll.radius, blastArea, Time.deltaTime * 5 );
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Enemy>())
        {
            Debug.Log("hit enemy:" + other);
            //other.ApplyExplosion(this);
        }
    }
}
