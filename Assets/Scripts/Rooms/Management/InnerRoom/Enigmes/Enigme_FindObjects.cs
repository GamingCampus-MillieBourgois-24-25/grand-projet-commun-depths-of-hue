using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enigme_FindObjects : Enigme
{
    public static Enigme_FindObjects Instance;

    [SerializeField] int maxRound = 3;
    public int currentRound = 0;

    public TextMeshProUGUI[] text;
    public GameObject panel;


    [SerializeField] private float timeLimit = 60f;
    private float timer; //current time

    

    [SerializeField] private List <GameObject> objectsInScene = new List<GameObject>(); // Every object possibly usable for the enigme
    [SerializeField] private List<GameObject> objectsUsedInEnigme; // Every object dynamically chose for the enigme

    [SerializeField] private int amountObjectsUsed = 3; // How many objects are used for this enigme

    public override void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
            base.Initialize();
        }

        ClearText();
        timer = timeLimit;
        objectsUsedInEnigme = MakeObjectsList();
        StartTimer();

        panel.SetActive(true);
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

        int io = 0;

        for (int i = 0; i < amountObjectsUsed && temp.Count > 0; i++) //Choose random items
        {
            

            int index = Random.Range(0, temp.Count);
            list.Add(temp[index]);
            temp.RemoveAt(index);

            text[io].text = list[io].gameObject.name;

            io++;

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

    /// <summary>
    /// This function checks if an item is part of the winning condition item list.
    /// Parameter expects an item.
    /// </summary>
    /// <param name="item"></param>
    public void CheckItem(GameObject item)
    {
        if (item == null) return;

  
        if (objectsUsedInEnigme.Contains(item))
        {
          
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].text == item.name)
                {
                    text[i].fontStyle = FontStyles.Strikethrough; 

                    objectsUsedInEnigme.Remove(item);

                    CheckEndOfRound();
                   

                    break;
                }
            }

            item.SetActive(false); 
        }
    }

    /// <summary>
    /// This function is used to check the end of the current round. It will react accordingy.
    /// </summary>
    private void CheckEndOfRound()
    {
        if (IsRoundEnded())
        {
            if (currentRound < (maxRound - 1))
            {
                currentRound++;
                Initialize(); //Restart a round
            }
            else
            {
                panel.SetActive(false);
                Success(); // End the enigme and invoke succes event

            }
        }

    }

    /// <summary>
    /// This function returns the state of the round ending. True = ended.
    /// </summary>
    /// <returns></returns>
    private bool IsRoundEnded()
    {
        return (objectsUsedInEnigme.Count == 0); 
    }

    private void ClearText()
    {
        for (int i = 0;i < text.Length; i++)
        {
            text[i].fontStyle = default;
        }
    }
}
