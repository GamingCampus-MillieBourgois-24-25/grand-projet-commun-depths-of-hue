using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[System.Serializable]
public class PositionData
{
    public string roomName;
    public Vector3 worldPosition;

    public PositionData(string roomName, Vector3 worldPosition)
    {
        this.roomName = roomName;
        this.worldPosition = worldPosition;
    }
}

public class Enigme_FindObjects : Enigme
{
    public static Enigme_FindObjects Instance;

    [SerializeField] int maxRound = 3;
    public int currentRound = 0;

    public TextMeshProUGUI[] text;
    public GameObject panel;


    [SerializeField] private float timeLimit = 60f;
    private float timer; //current time



    [SerializeField] private List<GameObject> objectsInScene; // Every object possibly usable for the enigme
    [SerializeField] private List<GameObject> objectsUsedInEnigme; // Every object dynamically chose for the enigme

    public List<PositionData> allObjectsPositions = new List<PositionData>();






    [SerializeField] private int amountObjectsUsed = 3; // How many objects are used for this enigme

    public override void Initialize()
    {
        if (Instance == null)
        {
            CollectAllPropsFromScene();
            CollectAllPositionsFromScene();

            Instance = this;

            base.Initialize();

            panel.SetActive(true);
        }

        ClearText();
        SetObject();
        timer = timeLimit;
        objectsUsedInEnigme = MakeObjectsList();
        StartTimer();

        
    }

    private void CollectAllPropsFromScene()
    {
        GameObject container = GameObject.FindWithTag("PropsContainer");

        if (container == null)
        {
            Debug.LogError("Le GameObject avec le tag 'PositionContainer' est introuvable !");
            return;
        }

        objectsInScene.Clear();

        foreach (Transform prop in container.transform)
        {
            objectsInScene.Add(prop.gameObject);
        }
    }
    private void CollectAllPositionsFromScene()
    {
        GameObject container = GameObject.FindWithTag("PositionContainer");

        if (container == null)
        {
            Debug.LogError("Le GameObject avec le tag 'PositionContainer' est introuvable !");
            return;
        }

        allObjectsPositions.Clear();

        foreach (Transform room in container.transform)
        {
            string roomName = room.name;

            foreach (Transform pos in room)
            {
                Vector3 worldPos = pos.position;
                allObjectsPositions.Add(new PositionData(roomName, worldPos + room.position));
            }
        }

        Debug.Log($"Nombre total de positions récupérées : {allObjectsPositions.Count}");
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
    public void CheckItem(ObjectEnigme item)
    {
        if (item == null) return;


        if (objectsUsedInEnigme.Contains(item.gameObject))
        {

            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].text == item.name)
                {
                    text[i].fontStyle = FontStyles.Strikethrough;

                    objectsUsedInEnigme.Remove(item.gameObject);

                    CheckEndOfRound();


                    break;
                }
            }

            item.MoveFragment(item.transform.position + new Vector3(-10, 0, 0), new Vector3(0.0f, 0.0f, 0.0f));
            //item.SetActive(false); 
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
                StartCoroutine(Wait(3)); 
                
            }
            else
            {
                panel.SetActive(false);
                Success(); // End the enigme and invoke succes event

            }
        }

    }

    /// <summary>
    /// Wait coroutine 
    /// Parameter exepcting a second amount
    /// </summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    IEnumerator Wait(float sec)
    {
        float elapsedTime = 0f;
        float duration = sec;

        while (elapsedTime < duration)
        {
           

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Initialize();
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
        for (int i = 0; i < text.Length; i++)
        {
            text[i].fontStyle = default;
        }
    }

    /// <summary>
    /// Reset objetcs position randomly + reset their scale
    /// </summary>
    private void SetObject()
    {
        var availablePositions = new List<PositionData>(allObjectsPositions);

        foreach (GameObject obj in objectsInScene)
        {
            if (availablePositions.Count == 0) break;

            int index = Random.Range(0, availablePositions.Count);
            PositionData posData = availablePositions[index];

            obj.transform.position = posData.worldPosition;
            obj.transform.localScale = Vector3.one;


            if (FramesManager.Instance != null)
            {
                FramesManager.Instance.AddFrameProp(posData.roomName, obj);
            }

            availablePositions.RemoveAt(index);
        }
    }
}
