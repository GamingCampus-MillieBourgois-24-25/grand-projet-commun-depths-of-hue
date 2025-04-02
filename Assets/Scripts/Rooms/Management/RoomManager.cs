    using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [Header("Data")]
    private RoomDataBase currentRoom;
    [SerializeField] private RoomDataBase[] allRooms;
    void Awake()
    {
        if (Instance == null) Instance = this; //Singleton instantiation
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        LoadRoom(allRooms[0].roomId);
    }


    /// <summary>
    /// Main function to load a scene/room.
    /// Parameter is expecting a float room ID.
    /// </summary>
    ///<param name="roomId"></param>
    public void LoadRoom(string roomId)
    {
        RoomDataBase targetRoom = System.Array.Find(allRooms, roomData => roomData.roomId == roomId); //Searching room function

        if (targetRoom == null)
        {
            Debug.LogError($"Room {roomId} not found!");
            return;
        }

        currentRoom = targetRoom;

        SceneManager.sceneLoaded += OnSceneLoaded; // On scene loaded event subscription
        SceneManager.LoadScene(targetRoom.sceneName); //Load the scene
    }


    /// <summary>
    /// Called when the event scene loaded is done
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (currentRoom != null)
        {
            if (scene.name != currentRoom.sceneName) return;

            // Finds the room controller script in the scene
            Room roomController = FindObjectOfType<Room>();

            if (roomController != null)
            {
                roomController.Initialize();
            }
            else
            {
                Debug.LogWarning("No RoomController found in scene!");
            }

            SceneManager.sceneLoaded -= OnSceneLoaded; // On scene loaded event unsubscription
        }
        
    }
}
