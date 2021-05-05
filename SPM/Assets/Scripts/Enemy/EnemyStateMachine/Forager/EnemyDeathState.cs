using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Death State", menuName = "New Enemy Death State")]
public class EnemyDeathState : State {

    private Forager forager;
    
    protected override void Initialize() {
        forager = (Forager) owner;
    }

    public override void Enter() {
        forager.Animator.SetTrigger("Die");
        forager.Pathfinder.agent.ResetPath();
        forager.Pathfinder.agent.isStopped = true;
    }

    public override void RunUpdate() {
        
        if (forager.activeBlackHole != null) {
            //Debug.Log("die");
            forager.transform.LookAt(forager.activeBlackHole.transform);

            forager.transform.position = Vector3.Lerp(forager.transform.position, forager.activeBlackHole.center.transform.position, Time.deltaTime);

            forager.transform.localScale = Vector3.Lerp(forager.transform.localScale, Vector3.zero, Time.deltaTime);
        }
        
        Destroy(forager.gameObject, 2.5f);
            
    }      
    
}
        