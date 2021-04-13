
using UnityEngine;

public class Forager : Enemy  {
    
    public float patrolAreaRadius;
    public float outerRing, innerRing;
    public LayerMask playerMask;
    public GameObject Bullet;
    
    [HideInInspector] public BlackHole activeBlackHole;

    private void Awake() {
        base.Awake();
    }
    private void Update() {
        base.Update();
        
        stateMachine?.RunUpdate();
    }

    public bool ProximityCast(float radius) {
        return Physics.OverlapSphere(transform.position, radius, playerMask).Length > 0;
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        if (pathfinder == null || !pathfinder.agent.hasPath) return;
        Gizmos.DrawLine(transform.position, pathfinder.agent.destination);
        Gizmos.DrawWireSphere(pathfinder.agent.destination, .5f);

    }
    
    public override void BlackHoleBehaviour(BlackHole blackHole) {
        if (activeBlackHole) return;
        activeBlackHole = blackHole;
        stateMachine.ChangeState<EnemyDeathState>(); }
    
    public void Fire() {
        Instantiate(Bullet, transform.position + transform.forward + Vector3.up, Quaternion.Euler(transform.forward));
        pathfinder.agent.isStopped = false;
    }
}
