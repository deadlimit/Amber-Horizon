using UnityEngine;

public class WallPiece : MonoBehaviour, IBlackHoleBehaviour  {

    private bool insideBlackHole;
    private BlackHole blackhole;
    public void BlackHoleBehaviour(BlackHole blackHole) {
        insideBlackHole = true;
        blackhole = blackHole;
    }

    private void Update() {
        if (!insideBlackHole || blackhole == null) return;
        
        transform.position = Vector3.Lerp(transform.position, blackhole.center.transform.position, Time.deltaTime * 10);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 10);
        
        Destroy(gameObject, 2);
            
    }
}
