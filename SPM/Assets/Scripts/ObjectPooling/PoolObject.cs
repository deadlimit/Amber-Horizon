using UnityEngine;

public abstract class PoolObject : MonoBehaviour {

    public virtual void Initialize(Vector3 position, Quaternion rotation) {
        transform.position = position;
        transform.rotation = rotation;
    }
}
