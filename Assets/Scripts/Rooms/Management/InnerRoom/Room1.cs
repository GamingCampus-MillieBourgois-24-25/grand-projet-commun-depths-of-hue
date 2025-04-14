using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1 : EnigmeRoom
{
    public override void Initialize()
    {
        base.Initialize();
        Debug.Log("YALALALA");
    }

    protected override void OnPostEnigme()
    {
        if (IsRoomComplete())
        {
            EndRoomSequence();
        }
        else
        {
            FramesManager.Instance.LockFrame("Cave");
        }
    }
}

