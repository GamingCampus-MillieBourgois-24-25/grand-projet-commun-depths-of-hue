using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MiddleRoom : EnigmeRoom
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        /*Initialize();*/
    }
    public override void Initialize()
    {
        base.Initialize();

    }

    public override void EndRoomSequence()
    {
        base.EndRoomSequence();

        ReturnToHub();
    }

}
