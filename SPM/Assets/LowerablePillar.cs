using System.Collections;
using UnityEngine;

public class LowerablePillar : MonoBehaviour {

    public float unitsDown;
    public float speed;
    private bool hasLowered;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") == false) return;

        print("player");
        if(!hasLowered)
        {
            hasLowered = true;
            StartCoroutine(Lower());
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
