using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class DeplacementPlayer : MonoBehaviour
{
    [Header("Property")]
    [SerializeField] private PathfindingGrid pathfindingGrid;
    [SerializeField] private NavMeshAgent navMeshAgent;
    
    [SerializeField] private Vector3 playerDestination;

    private Camera _camera;
    
    private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnEnable()
    {
        TouchSimulation.Enable();
    }

    private void Start()
    {
        Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
        _camera = Camera.main;
    }

    private void AssignPositionsInTheGridToUnits()
    {
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

    private void Update()
    {
        foreach (var touch in Touch.activeTouches)
        {
            if (touch.isTap)
            {
                Vector2 touchPosition = touch.screenPosition;
                RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(touchPosition), Vector2.zero);
                if (hit.collider)
                {
                    Debug.Log("Objet touché : " + hit.collider.gameObject.name);
                    MonoBehaviour script = hit.collider.GetComponent<MonoBehaviour>();

                    if (script != null)
                    {
                        script.Invoke("OnObjectClicked", 0f);
                    }
                    else
                    {
                        print("il n'y a rien");
                    }
                }
            }
        }
    }
}
