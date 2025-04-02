using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    public static HubManager Instance;

    [Header("References")]
    [SerializeField] private DoorController[] doors;
    [SerializeField] private GameObject playerSpawnPoint;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeDoors();
    }

    /// <summary>
    /// Initialize each door in the room, resulting in a state update + visual update
    /// </summary>
    void InitializeDoors()
    {
        if(doors != null)
        {        
            foreach (DoorController door in doors)
            {
                door.Initialize(); // Initialize each door
            }
        }
    }
}
