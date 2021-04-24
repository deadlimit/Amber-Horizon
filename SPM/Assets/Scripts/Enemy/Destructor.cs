using UnityEngine;

public class Destructor : Enemy {

    public TextMesh text;
    
    private void Start() {
        stateMachine.ChangeState<DestructorPatrolState>();
    }
    
    private void Update() {
        base.Update();
        
        stateMachine.RunUpdate();
        text.text = stateMachine.currentState.ToString();

    }

    /*private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        if (Pathfinder != null  && Pathfinder.agent.destination == Vector3.zero) return;
        Gizmos.DrawLine(transform.position, Pathfinder.agent.destination);
        Gizmos.DrawWireSphere(Pathfinder.agent.destination, 1);
        Gizmos.DrawWireSphere(transform.position, outerRing);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, innerRing);
    }*/
}
