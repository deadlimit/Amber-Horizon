using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Teleport State", menuName = "New Enemy Teleport State")]
public class EnemyTeleportState : State {

    private Forager forager;

    public float distanceFromPlayer;
    
    protected override void Initialize() {
        forager = (Forager) owner;
    }

    public override void Enter() {
        forager.Animator.StopPlayback();
        forager.Animator.SetTrigger("Teleport");


        Vector3 randomPosition = forager.Target.transform.position + new Vector3(
            Random.Range(-distanceFromPlayer, distanceFromPlayer),
            Random.Range(-distanceFromPlayer, distanceFromPlayer),
            Random.Range(-distanceFromPlayer, distanceFromPlayer));
 
        forager.Invoke(() => {
            forager.transform.position = randomPosition;
            forager.stateMachine.ChangeState<EnemyProximityState>();
        }, .2f);
    }
}
