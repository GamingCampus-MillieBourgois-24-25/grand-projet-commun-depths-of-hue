using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    [Header("Property")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform targetPlayer;

    private void Start()
    {
        if (!navMeshAgent) return;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    private void OnEnable()
    {
        GestionCadre.OnSendInfoPlayerMovement += SetNewDestination;
    }
    
    private void OnDisable()
    {
        GestionCadre.OnSendInfoPlayerMovement -= SetNewDestination;
    }
    
    private void SetNewDestination()
    {
        if (!navMeshAgent) return;
        navMeshAgent.SetDestination(targetPlayer.position);
    }

    private void Update()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(targetPlayer.position);
        }
    }
}
