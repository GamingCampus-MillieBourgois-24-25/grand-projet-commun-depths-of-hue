using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
    public List<ItemData> allItems;

    void Start()
    {
        string searchTerm = "perle";
        List<ItemData> matchingItems = FindItemsByPartialName(searchTerm);

        foreach (ItemData item in matchingItems)
        {
            Debug.Log("Objet trouvé : " + item.itemName);
        }
    }

    public void Add(ItemData item)
    {
        if (!allItems.Contains(item))
        {
            allItems.Add(item);
            Debug.Log(item.itemName + " ajouté à l'inventaire.");
        }
        else
        {
            Debug.Log(item.itemName + " est déjà dans l'inventaire.");
        }
    }

    public void Remove(ItemData item)
    {
        if (allItems.Contains(item))
        {
            allItems.Remove(item);
            Debug.Log(item.itemName + " retiré de l'inventaire.");
        }
        else
        {
            Debug.Log(item.itemName + " n'est pas dans l'inventaire.");
        }
    }

    public List<ItemData> FindItemsByPartialName(string partialName)
    {
        List<ItemData> foundItems = allItems.Where(item => item.itemName.IndexOf(partialName, System.StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        if (foundItems.Count == 0)
        {
            throw new System.Exception("Aucun objet trouvé contenant : " + partialName);
        }
        return foundItems;
    }

    public ItemData GetItemByName(string itemName)
    {
        ItemData item = allItems.FirstOrDefault(item => item.itemName.Equals(itemName, System.StringComparison.OrdinalIgnoreCase));
        if (item == null)
        {
            throw new System.Exception("Aucun objet trouvé avec le nom : " + itemName);
        }
        return item;
    }

    public List<Sprite> GetAllSprites()
    {
        if (allItems.Count == 0)
        {
            throw new System.Exception("L'inventaire est vide, aucun sprite disponible.");
        }
        List<Sprite> sprites = allItems.Select(item => item.itemSprite).Where(sprite => sprite != null).ToList();
        if (sprites.Count == 0)
        {
            throw new System.Exception("Aucun sprite valide trouvé dans l'inventaire.");
        }
        return sprites;
    }
}
