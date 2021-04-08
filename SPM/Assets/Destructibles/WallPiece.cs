using UnityEngine;

public class WallPiece : MonoBehaviour, IBlackHoleBehaviour {
    
    public void BlackHoleBehaviour(BlackHole blackHole) {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        transform.parent = null;
        print("wall piece");
    }
}
