using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Recorder.OutputPath;

public class EnigmeRoom : Room
{
    [SerializeField] private List<Enigme> enigmes;
    private int enigmesResolved = 0;

    [ContextMenu("Initialize")]


    /// <summary>
    /// This override is used intialize the room.
    /// </summary>
    public override void Initialize()
    {
        foreach (var enigme in enigmes)// subscribe to each enigme OnSucces event.
        {
            enigme.OnSuccess += OnEnigmeResolved;
        }
        InitilizeCurrentEnigma();
    }

    /// <summary>
    /// This function is used to initialize and launch the first enigme not resolved yet in the enigmas list.
    /// </summary>
    protected virtual void InitilizeCurrentEnigma()
    {
        foreach (var enigme in enigmes)
        {
            if (enigme.GetIsResolved())
            {
                continue;
            }
            else
            {
                enigme.Initialize();
                break;
            }
        }
    }

    /// <summary>
    /// This function is called whenever an enigme is resolved.
    /// </summary>
    private void OnEnigmeResolved()
    {
        enigmesResolved++;

        if  (IsRoomComplete())
        {
            EndRoomSequence();
        }
        else
        {
            InitilizeCurrentEnigma();
        }
        
    }

    /// <summary>
    /// Detects if the room has enigmes left. False = not all enigmes done.
    /// </summary>
    /// <returns></returns>
    public bool IsRoomComplete()
    {
        return enigmesResolved >= enigmes.Count;
    }


    /// <summary>
    /// End of room sequence. (logique..)
    /// </summary>
    public virtual void EndRoomSequence()
    {
        roomData.CurrentState = RoomStateEnum.Completed;
        roomData.roomState = roomData.CurrentState;

    }
}
