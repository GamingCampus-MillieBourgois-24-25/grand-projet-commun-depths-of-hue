using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiddleRoom : EnigmeRoom
{
    // Start is called before the first frame update
    private void Start()
    {
        Initialize();
    }
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
