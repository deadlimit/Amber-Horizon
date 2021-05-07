using UnityEngine;
[CreateAssetMenu(fileName = "Enemy Distance State", menuName = "New Enemy Distance State")]
public class EnemyDistanceState : State {

    private Forager forager;

    protected override void Initialize() {
        forager = (Forager) owner;
    }
    public override void Enter()
    {
        Debug.Log("Distance state");
    }
    public override void RunUpdate() {
        forager.transform.LookAt(forager.Target);
        if(forager.ProximityCast(forager.outerRing))
            stateMachine.ChangeState<EnemyProximityState>();
        
    }
    
}
