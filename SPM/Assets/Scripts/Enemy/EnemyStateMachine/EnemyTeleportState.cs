using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Teleport State", menuName = "New Enemy Teleport State")]
public class EnemyTeleportState : State {

    private Forager forager;

    public float distanceFromPlayer;
    
    protected override void Initialize() {
        forager = (Forager) owner;
    }

    public override void Enter() {
        Debug.Log("enter");
        forager.animator.StopPlayback();
        forager.animator.SetTrigger("Teleport");
        
        //Hitta en ny plats en distans from spelaren
        Vector3 randomPosition = forager.transform.position + (Vector3)Random.insideUnitCircle * distanceFromPlayer;

        //Hitta en position på navmeshen så fienden inte försvinner in i en vägg eller något. 
        Vector3 newPosition = forager.pathfinder.GetSamplePositionOnNavMesh(randomPosition, 1);

        Debug.Log(newPosition);
        
        forager.Invoke(() => {
            forager.transform.Translate(newPosition);
            forager.stateMachine.ChangeState<EnemyProximityState>();
        }, .2f);
    }
}
