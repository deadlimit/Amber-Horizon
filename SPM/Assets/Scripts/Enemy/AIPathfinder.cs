using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIPathfinder : MonoBehaviour {

    public NavMeshAgent agent { get; private set; }

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }
    
    public Vector3 GetSamplePositionOnNavMesh(Vector3 origin, float originRadius, float maxDistance) {
        Vector3 randomPositionInsidePatrolArea = Random.insideUnitSphere * originRadius + origin;
        NavMesh.SamplePosition(randomPositionInsidePatrolArea, out var hitInfo, maxDistance, NavMesh.AllAreas);
        return hitInfo.position;
    }
    
}
