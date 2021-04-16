using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIPathfinder : MonoBehaviour
{

    public NavMeshAgent agent { get; private set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public Vector3 GetSamplePositionOnNavMesh(Vector3 origin, float originRadius)
    {
        Vector3 randomPositionInsidePatrolArea = Random.insideUnitSphere * originRadius + origin;
        NavMesh.SamplePosition(randomPositionInsidePatrolArea, out var hitInfo, 10, NavMesh.AllAreas);
        return hitInfo.position;
    }

}
