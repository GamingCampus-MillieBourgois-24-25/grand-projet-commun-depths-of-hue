using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
    public List<GameObject> allObjects;

    void Start()
    {
        string searchTerm = "perle";
        List<GameObject> matchingObjects = FindObjectsByPartialName(searchTerm);

        foreach (GameObject obj in matchingObjects)
        {
            Debug.Log("Objet trouvé : " + obj.name);
        }
    }

    public void Add(GameObject obj)
    {
        if (!allObjects.Contains(obj))
        {
            allObjects.Add(obj);
            Debug.Log(obj.name + " ajouté à l'inventaire.");
        }
        else
        {
            Debug.Log(obj.name + " est déjà dans l'inventaire.");
        }
    }

    public void Remove(GameObject obj)
    {
        if (allObjects.Contains(obj))
        {
            allObjects.Remove(obj);
            Debug.Log(obj.name + " retiré de l'inventaire.");
        }
        else
        {
            Debug.Log(obj.name + " n'est pas dans l'inventaire.");
        }
    }

    public List<GameObject> FindObjectsByPartialName(string partialName)
    {
        return allObjects.Where(obj => obj.name.ToLower().Contains(partialName.ToLower())).ToList();
    }

    public void PrintInventory()
    {
        Debug.Log("Inventaire : " + (allObjects.Count > 0 ? string.Join(", ", allObjects.Select(obj => obj.name)) : "Vide"));
    }


}
