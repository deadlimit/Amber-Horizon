using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[CreateAssetMenu]
public class EnemyIdleState : State {

    [SerializeField] private List<Vector3> patrolPoints;

    private Queue<Vector3> internalPatrolPoints;
    
    private Enemy thisEnemy;
    
    private bool patrolling;

    private Vector3 destination;
    
    protected override void Initialize() {
        thisEnemy = (Enemy) owner;
        patrolling = false;
        internalPatrolPoints = new Queue<Vector3>(patrolPoints);
    }

    public override void RunUpdate() {

        if (patrolling) 
            Patrol();
        else
            SetNewPatrol();
        

    }


    private void Patrol() {
        if (Vector3.Distance(thisEnemy.transform.position, destination) < 2)
            patrolling = false;
        
    }

    private void SetNewPatrol() {
        
        destination = internalPatrolPoints.Peek();
        thisEnemy.MeshAgent.SetDestination(destination);

        //Lägger första vectorn längst bak i kön
        Vector3 frontVector = internalPatrolPoints.Dequeue();
        internalPatrolPoints.Enqueue(frontVector);

        patrolling = true;
        Debug.Log(internalPatrolPoints.Peek());
    }


}
