using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform target;
    UnityEngine.AI.NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        agent.SetDestination(target.position);
    }
}
