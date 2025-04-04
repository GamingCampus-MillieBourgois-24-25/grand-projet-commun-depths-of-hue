    using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        LoadAllRooms();
    }


    /// <summary>
    /// Main function to load a scene/room.
    /// Parameter is expecting a string room ID.
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
    /// Detects every Room data base automatically. Only works in editor mode !!!!
    /// </summary>
    void LoadAllRooms()
    {
#if UNITY_EDITOR
        // Méthode Éditeur (plus complète)
        string[] guids = AssetDatabase.FindAssets("t:RoomDataBase");
        allRooms = new RoomDataBase[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            allRooms[i] = AssetDatabase.LoadAssetAtPath<RoomDataBase>(path);
        }
#else
        // Méthode Build - Solution 1 (si dans Resources)
        allRooms = Resources.LoadAll<RoomDataBase>("");
        
        // OU Solution 2 (préchargement recommandé)
        // Laissez le tableau sérialisé et rempli manuellement
        // (voir alternative ci-dessous)
#endif

        // Trie commun
        if (allRooms != null)
        {
            System.Array.Sort(allRooms, (a, b) => a.roomId.CompareTo(b.roomId));
        }
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
                UnlockRoom("0");
            }
            else
            {
                Debug.LogWarning("No RoomController found in scene!");
            }

            SceneManager.sceneLoaded -= OnSceneLoaded; // On scene loaded event unsubscription
        }
        
    }

    /// <summary>
    /// Getter for room data.
    /// Paremeter is expecting a string room ID.
    /// </summary>
    /// <param name="roomId"></param>
    /// <returns></returns>
    public RoomDataBase GetRoomData(string roomId)
    {
        RoomDataBase targetRoom = System.Array.Find(allRooms, roomData => roomData.roomId == roomId); //Searching room function
        if (targetRoom == null)
        {
            Debug.LogError($"Room {roomId} not found!");
            return null;
        }

        return targetRoom;
    }


    /// <summary>
    /// Unlocks a room.
    /// Parameter is expecting a string room ID
    /// </summary>
    /// <param name="roomId"></param>
    public void UnlockRoom(string roomId)
    {

        RoomDataBase room = GetRoomData(roomId);
        if (room != null)
        {
            room.CurrentState = RoomStateEnum.Unlocked;
            Debug.Log($"Room {roomId} unlocked !");

            room.roomState = room.CurrentState;
            Debug.Log(room.CurrentState.ToString());
        }
    }
}
