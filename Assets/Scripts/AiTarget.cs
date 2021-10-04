using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AiTarget : Target
{
    NavMeshAgent agent;                
    private void OnEnable()
    {
        // Set target to random position with method from base - Target)

        transform.position = base.GetRandomValidPoint();
        
        // Set random valid destination on navmesh surface

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(GetRandomValidPoint());
    }

    void Update()
    {
        // If agent is close to destination, change destination

        if (Vector3.Distance(transform.position, agent.destination) < 1f)
        {
            agent.SetDestination(GetRandomValidPoint());
        }
    }

    public override Vector3 GetRandomValidPoint()
    {
        GameObject arena = GameManager.Instance.arena.transform.Find("Floor").gameObject;

        float maxFloorDistance = arena.GetComponent<Collider>().bounds.size.x;

        // Find valid random point on arena surface

        Vector3 sourcePosition = Random.insideUnitSphere * maxFloorDistance + transform.position;
        NavMesh.SamplePosition(sourcePosition, out NavMeshHit hit, maxFloorDistance, NavMesh.AllAreas);

        return hit.position;
    }
}
