using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeplacementPlayer : MonoBehaviour
{
    [Header("Property")]
    [SerializeField] private PathfindingGrid pathfindingGrid;
    [SerializeField] private NavMeshAgent navMeshAgent;
    
    [SerializeField] private Vector3 playerDestination;

    private void Start()
    {
        AssignPositionsInTheGridToUnits();
    }

    private void AssignPositionsInTheGridToUnits()
    {
//        Vector3 _targetPosition = GetAvailableGridPosition(playerDestination);
        navMeshAgent.SetDestination(playerDestination);
    }
    
    private Vector3 GetAvailableGridPosition(Vector3 _targetPosition)
    {
        return IsPositionWalkable(_targetPosition) ? _targetPosition : Vector3.zero;
    }
    
    private bool IsPositionWalkable(Vector3 position)
    {
        Vector3Int cellPosition = Vector3Int.FloorToInt(position);
        return pathfindingGrid && pathfindingGrid.IsWalkable(cellPosition);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(playerDestination, Vector3.one * 1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(playerDestination, 0.3f);
    }
}
