using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private string targetRoomId;
    [SerializeField] private bool isLocked;
    [SerializeField] private GameObject lockedVisual;
    [SerializeField] private GameObject unlockedVisual;


    /// <summary>
    /// Called by HUB manager, initialize door depending on its state
    /// </summary>
    public void Initialize()
    {
        //Check room's state
        RoomDataBase targetRoom = RoomManager.Instance.GetRoomData(targetRoomId);
        isLocked = (targetRoom.roomStateEnum != RoomStateEnum.Unlocked);

        // Update visuals
        UpdateDoorVisual();

        // Debug or in game message display
        Debug.Log($"Porte {targetRoomId} initialis�e : Verrouill�e = {isLocked}");
    }


    /// <summary>
    /// Update the door visuals.
    /// </summary>
    private void UpdateDoorVisual()
    {
        if (lockedVisual != null) lockedVisual.SetActive(isLocked);
        if (unlockedVisual != null) unlockedVisual.SetActive(!isLocked);
    }

    /// <summary>
    /// Called when door is clicked. If not locked, will load the attached room.
    /// </summary>
    public void OnClicked()
    {
        if (!isLocked)
        {
            RoomManager.Instance.LoadRoom(targetRoomId);
        }
        else
        {
            Debug.Log("Door Locked !");
        }
    }
}
