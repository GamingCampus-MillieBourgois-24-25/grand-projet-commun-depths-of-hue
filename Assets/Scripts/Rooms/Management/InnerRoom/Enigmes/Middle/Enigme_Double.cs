using TMPro;
using System.Collections.Generic;
using UnityEngine;

public class Enigme_Doble : Enigme
{
    private GameObject firstSelected;
    private GameObject secondSelected;
    private int nbDouble;
    [SerializeField] private List<GameObject> ItemsDouble;
    [SerializeField] private Camera cam;
    [SerializeField] private Material materialToApply;

    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject zoneText;

    public override void Initialize()
    {
        base.Initialize();
        nbDouble = ItemsDouble.Count / 2;
        SpawnObjects();
        UpdateTexte();
        FramesManager.Instance.LockFrame("Pillar");
    }

    public void ObjectClicked(GameObject obj)
    {
        if (obj == firstSelected) return;

        if (firstSelected == null)
        {
            Material outlineMaterial = new Material(materialToApply);
            firstSelected = obj;
            firstSelected.GetComponent<Outline>().enabled = true;
        }
        else
        {
            secondSelected = obj;
            CheckObj();
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
            firstSelected.GetComponent<Outline>().enabled = false;
            firstSelected.SetActive(false);
            secondSelected.SetActive(false);
            firstSelected = null;
            secondSelected = null;
            nbDouble--;
            UpdateTexte();
            Success();
        }
        else
        {
            firstSelected.GetComponent<Outline>().enabled = false;
            firstSelected = null;
            secondSelected = null;
        }
    }

    protected override void Success()
    {
        if(nbDouble == 0 && !isResolved)
        {
            base.Success();
       
            FramesManager.Instance.LockFrame("main_frame");
            FramesManager.Instance.UnlockFrame("Pillar");
            zoneText.SetActive(false);
        }
    }

    /*public override void UpdateEnigme(float deltaTime)
    {
        Success();
    }

    private void Update()
    {
        if (isStarted)
        {

            UpdateEnigme(Time.deltaTime);
        }
    }*/

    public void SpawnObjects()
    {
        Vector3 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        float minX = bottomLeft.x + 1f;
        float maxX = topRight.x - 1f;
        float minY = bottomLeft.y + 1f;
        float maxY = topRight.y - 1f;

        List<Vector2> occupiedPositions = new List<Vector2>();

        foreach (var obj in ItemsDouble)
        {
            Vector2 spawnPosition = GetRandomPosition(minX, maxX, minY, maxY, occupiedPositions);
            if (spawnPosition != Vector2.zero)
            {
                Vector3 newPosition = new Vector3(spawnPosition.x, spawnPosition.y, obj.transform.position.z);
                obj.transform.position = newPosition;
                obj.GetComponent<ItemDouble>().enigmeD = this;
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
}
