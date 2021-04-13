
using UnityEngine;

public class Forager : Enemy  {
    


    public GameObject Bullet;
    
    [HideInInspector] public BlackHole activeBlackHole;

    private void Awake() {
        base.Awake();
    }
    private void Update() {
        base.Update();
        
        stateMachine?.RunUpdate();
    }
    
    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        if (Pathfinder == null || !Pathfinder.agent.hasPath) return;
        Gizmos.DrawLine(transform.position, Pathfinder.agent.destination);
        Gizmos.DrawWireSphere(Pathfinder.agent.destination, .5f);

    }
    public override void BlackHoleBehaviour(BlackHole blackHole) {
        if (activeBlackHole) return;
        activeBlackHole = blackHole;
        stateMachine.ChangeState<EnemyDeathState>(); }
    
    public void Fire() {
        Instantiate(Bullet, transform.position + transform.forward + Vector3.up, Quaternion.Euler(transform.forward));
        Pathfinder.agent.isStopped = false;
    }
}
