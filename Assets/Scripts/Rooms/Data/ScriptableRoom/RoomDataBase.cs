using UnityEngine;

[CreateAssetMenu(fileName = "Room_", menuName = "Game/Rooms/Data", order = 1)]
public class RoomDataBase : ScriptableObject
{
    [Header("Identification")]
    public string roomId;
    public string sceneName;

    [Header("Default State")]
    public RoomStateEnum initialState;

    public RoomStateEnum roomState;

    // Propriété avec sauvegarde automatique
    public RoomStateEnum CurrentState
    {
        get => (RoomStateEnum)PlayerPrefs.GetInt(roomId + "_state", (int)initialState);
        set => PlayerPrefs.SetInt(roomId + "_state", (int)value);

    }
}
