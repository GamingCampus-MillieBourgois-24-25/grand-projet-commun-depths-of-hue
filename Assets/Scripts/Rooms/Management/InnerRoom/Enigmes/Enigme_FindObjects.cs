using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enigme_FindObjects : Enigme
{
    
    [SerializeField] private float timeLimit = 60f;
    private float timer; //current time

    

    [SerializeField] private List <GameObject> objectsInScene = new List<GameObject>(); // Every object possibly usable for the enigme
    [SerializeField] private List<GameObject> objectsUsedInEnigme; // Every object dynamically chose for the enigme

    [SerializeField] private int amountObjectsUsed = 3; // How many objects are used for this enigme

    public override void Initialize()
    {
        base.Initialize();
        timer = timeLimit;
        objectsUsedInEnigme = MakeObjectsList();
        StartTimer();
    }

    void StartTimer()
    {
        if (EnigmaTimerManager.Instance != null)
        {
            EnigmaTimerManager.Instance.ShowTimer();
        }
    }

    /// <summary>
    /// This function chose random items and return a list of these objects. List size depends on amountObjectsUsed variable.
    /// </summary>
    private List<GameObject> MakeObjectsList()
    {
        List<GameObject> list = new List<GameObject>(); //Temporary list
        var temp = new List<GameObject>(objectsInScene); // Clone objects in scene (to remove elements)

        for (int i = 0; i < amountObjectsUsed && temp.Count > 0; i++) //Chose random items
        {
            int index = Random.Range(0, temp.Count);
            list.Add(temp[index]);
            temp.RemoveAt(index);
        }

        return list;

    }

    private void Update()
    {
        if (isStarted)
        {
            
            UpdateEnigme(Time.deltaTime);
        }
    }
    public override void UpdateEnigme(float deltaTime)
    {
        
        if (isResolved) return;


        timer -= deltaTime;

        if (EnigmaTimerManager.Instance != null)
        {
            EnigmaTimerManager.Instance?.UpdateTimerDisplay(timer); //Updates the timer display
        }
        

        if (timer <= 0f)
        {
            Fail();
        }
    }
}
