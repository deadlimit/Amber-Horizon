using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderKeg : MonoBehaviour
{
    public LayerMask collisionMask;
    private SphereCollider coll;
    private float blastArea = 10f;
    private void Awake()
    {
        coll = GetComponent<SphereCollider>();

    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if (collision.gameObject.layer == 9)
        {
            Debug.Log("collisionlayer");
            Explode();
        }
    }


    private void Explode() 
    {
        Debug.Log("Boom!");
        StartCoroutine(ExpandRadius(coll.radius));
        Invoke("Despawn", 1f);
    }
    private IEnumerator ExpandRadius(float targetRadius)
    {
        Debug.Log(targetRadius < blastArea);
        while (targetRadius < blastArea)
        {
            coll.radius = Mathf.Lerp(coll.radius, blastArea, Time.deltaTime * 5);
            yield return null;
        }
    }
    private void Despawn() 
    {
        Debug.Log("despawning");
        Destroy(this);
    }

}
