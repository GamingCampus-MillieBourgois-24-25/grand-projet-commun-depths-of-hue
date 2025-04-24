using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room2 : EnigmeRoom
{
    [SerializeField] private GameObject mapCanvas;
    [SerializeField] private GameObject SudokuCanvas;
    [SerializeField] private GameObject SoundCanvas;

    [SerializeField] private GameObject sirene;


    public override void Initialize()
    {
        

        foreach (var enigme in enigmes)// subscribe to each enigme OnSucces event.
        {
            enigme.OnSuccess -= OnEnigmeResolved; // if already subscribed
            enigme.OnSuccess += OnEnigmeResolved;
        }
    }
    public void InitializeSpecificEnigme(int enigme)
    {
        mapCanvas.SetActive(false);
        SudokuCanvas.SetActive(false);
        SoundCanvas.SetActive(false);

        enigmes[enigme].Initialize();
    }
}
