using System.Collections;
using UnityEngine;

public class LowerablePillar : MonoBehaviour {

    public float unitsDown;
    public float speed;
    public bool started;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") == false) return;

        print("player");
        if(started ==false)
        {
            StartCoroutine(Lower());
            started = true;
        }
    }

    private IEnumerator Lower() {

        Vector3 distanceDown = transform.position + Vector3.down * unitsDown;

        while (transform.position.y > distanceDown.y + +.3f) {
            transform.position = Vector3.Lerp(transform.position, distanceDown, Time.deltaTime * speed);
            yield return null;
        }

        print("done");
    }
}
