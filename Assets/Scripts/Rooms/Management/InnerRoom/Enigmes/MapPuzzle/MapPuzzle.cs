using System.Collections.Generic;
using UnityEngine;

public class MapPuzzle : Enigme
{
    public List<MapLocations> rightOrderList = new List<MapLocations>();
    public List<GameObject> chosenOrderList = new List<GameObject>();
    private List<LineRenderer> listLineRenderer = new List<LineRenderer>();

    public GameObject prefabLine;
    public static MapPuzzle Instance;

    public override void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        base.Initialize();
    }
    
    public void AddToChosenList(GameObject newObject)
    {
        Location objLocation = newObject.GetComponent<Location>();
        if (!objLocation || objLocation.isVisited) return;
        chosenOrderList.Add(newObject);
        objLocation.VisitLoc();
        if (chosenOrderList.Count > 1)
        {
            GameObject obj1 = chosenOrderList[^2];
            GameObject obj2 = chosenOrderList[^1];
            Vector3[] pos =
            {
                new Vector3(obj1.transform.position.x,obj1.transform.position.y,89),
                new Vector3(obj2.transform.position.x,obj2.transform.position.y,89)
            };
            LineRenderer lineRenderer = Instantiate(prefabLine).GetComponent<LineRenderer>();
            listLineRenderer.Add(lineRenderer);
            lineRenderer.SetPositions(pos);
            lineRenderer.gameObject.SetActive(true);
        }
        CheckOrder();
    }

    public void RemoveLastPosition()
    {
        switch (chosenOrderList.Count)
        {
            case <= 0:
                return;
            case >= 2:
            {
                LineRenderer line = listLineRenderer[^1];
                listLineRenderer.Remove(line);
                Destroy(line);
                break;
            }
        }

        chosenOrderList[^1].GetComponent<Location>().UnvisitLoc();
        chosenOrderList.Remove(chosenOrderList[^1]);
    }

    private void CheckOrder()
    {
        if (rightOrderList.Count != chosenOrderList.Count)
        {
            return;
        }

        for (int i = 0; i < rightOrderList.Count; i++)
        {
            if (rightOrderList[i] != chosenOrderList[i].GetComponent<Location>().locationName)
            {
                return;
            }
        }
        Debug.Log("WIN");
    }
}
