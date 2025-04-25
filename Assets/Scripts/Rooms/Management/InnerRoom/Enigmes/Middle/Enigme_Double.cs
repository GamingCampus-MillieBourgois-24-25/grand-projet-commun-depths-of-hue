using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class Enigme_Doble : Enigme
{
    private GameObject firstSelected;
    private GameObject secondSelected;
    private GameObject firstBulleSelected;
    private GameObject secondBulleSelected;
    private int nbDouble;
    [SerializeField] private List<GameObject> ItemsDouble;
    [SerializeField] private Camera cam;
    [SerializeField] private ImgBackGroundEnigme img;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject zoneText;
    [SerializeField] private GameObject bubulle;
    [SerializeField] private GestionInputs gestion;
    private List<GameObject> bulles;
    private float timer = 1f; // 1 seconde
    private bool isTimerRunning = false;

    private void Start()
    {
        
    }
    public override void Initialize()
    {
        base.Initialize();
        nbDouble = ItemsDouble.Count / 2;
        CreateBulle();
        SpawnObjects();
        UpdateTexte();
        Vector3 vector = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 5);
        img.transform.position = vector;
        FramesManager.Instance.LockFrame("Pillar");
    }

    public void ObjectClicked(GameObject obj)
    {
        Bulle bubulle = obj.GetComponent<Bulle>();
        if (bubulle.item == firstSelected) return;

        if (firstSelected == null && firstBulleSelected == null)
        {
            /*Material outlineMaterial = new Material(materialToApply);*/
            firstSelected = bubulle.item;
            firstBulleSelected = obj;
            bubulle.item.transform.position = obj.transform.position;
            bubulle.item.SetActive(true);
            obj.SetActive(false);
        }
        else
        {
            secondSelected = bubulle.item;
            secondBulleSelected = obj;
            bubulle.item.transform.position = obj.transform.position;
            bubulle.item.SetActive(true);
            obj.SetActive(false);
            isTimerRunning = true;
            gestion.enabled = false;
        }
    }


    void Update()
    {
        if (isTimerRunning)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Debug.Log("Timer terminé !");
                isTimerRunning = false;
                CheckObj();
                timer = 1f;
            }
        }
    }

    public void UpdateTexte()
    {
        text.text = nbDouble.ToString();
    }

    public void CheckObj()
    {
        ItemDouble firstObj = firstSelected.GetComponent<ItemDouble>();
        ItemDouble secondObj = secondSelected.GetComponent<ItemDouble>();
        if(firstObj == null || secondObj == null)
        {
            return;
        }
        if (firstObj.GetId() == secondObj.GetId())
        {
            firstSelected.SetActive(false);
            secondSelected.SetActive(false);
            firstSelected = null;
            secondSelected = null;
            firstBulleSelected = null;
            secondBulleSelected = null;
            nbDouble--;
            UpdateTexte();
            Success();
            gestion.enabled = true;
        }
        else
        {
            firstSelected.SetActive(false);
            secondSelected.SetActive(false);
            firstBulleSelected.SetActive(true);
            secondBulleSelected.SetActive(true);
            firstBulleSelected = null;
            secondBulleSelected = null;
            firstSelected = null;
            secondSelected = null;
            gestion.enabled = true;

        }
    }

    protected override void Success()
    {
        if(nbDouble == 0 && !isResolved)
        {
            base.Success();
            zoneText.SetActive(false);
        }
    }


    public void SpawnObjects()
    {
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        float minX = bottomLeft.x + 1f;
        float maxX = topRight.x - 1f;
        float minY = bottomLeft.y + 1f;
        float maxY = topRight.y - 1f;

        List<Vector2> occupiedPositions = new List<Vector2>();

        foreach (var obj in bulles)
        {
            Vector2 spawnPosition = GetRandomPosition(minX, maxX, minY, maxY, occupiedPositions);
            if (spawnPosition != Vector2.zero)
            {
                Vector3 newPosition = new Vector3(spawnPosition.x, spawnPosition.y, obj.transform.position.z);
                obj.transform.position = newPosition;
                occupiedPositions.Add(spawnPosition);
            }
        }
    }


    public Vector2 GetRandomPosition(float minX, float maxX, float minY, float maxY, List<Vector2> occupiedPositions)
    {
        Vector2 position;
        bool isValidPosition = false;

        while (!isValidPosition)
        {
            position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            isValidPosition = true;

            foreach (var occupied in occupiedPositions)
            {
                if (Vector2.Distance(position, occupied) < 2f)
                {
                    isValidPosition = false;
                    break;
                }
            }

            if (isValidPosition)
            {
                return position;
            }
        }

        return Vector2.zero;
    }

    public override void CheckItem(GameObject item)
    {
        base.CheckItem(item);
        ObjectClicked(item);
    }

    public void CreateBulle()
    {
        if (ItemsDouble == null || ItemsDouble.Count == 0)
        {
            Debug.LogError("ItemsDouble est null ou vide !");
            return;
        }

        if (bubulle == null)
        {
            Debug.LogError("Le prefab bubulle n'est pas assigné !");
            return;
        }
        if (bulles == null)
        {
            bulles = new List<GameObject>();
            Debug.LogWarning("La liste bulles était null et a été initialisée.");
        }

        List<GameObject> list = new List<GameObject>(ItemsDouble);
        for (int i = 0; i < ItemsDouble.Count; i++)
        {
            if (list.Count == 0)
            {
                Debug.LogWarning("La liste est vide avant la fin de la boucle !");
                break;
            }

            int index = Random.Range(0, list.Count);
            GameObject nuwBulle = Instantiate(bubulle, Vector3.zero, Quaternion.identity);

            Bulle bulleScript = nuwBulle.GetComponent<Bulle>();
            if (bulleScript == null)
            {
                Debug.LogError("Le composant Bulle est manquant sur le prefab bubulle !");
                Destroy(nuwBulle);
                continue;
            }

            bulleScript.item = list[index];
            list.RemoveAt(index);
            bulles.Add(nuwBulle);
        }
    }
}

