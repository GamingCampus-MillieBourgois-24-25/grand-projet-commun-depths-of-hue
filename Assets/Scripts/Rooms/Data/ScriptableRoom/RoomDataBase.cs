using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "Rooms/Room_data", order = 1)]
public class RoomDataBase : ScriptableObject
{
    [SerializeField] public string roomId; // First room id = 0
    [SerializeField] public string sceneName; //Format : Room_roomNumber .Exemple : Room_1
    [SerializeField] public RoomStateEnum roomStateEnum; 

}
