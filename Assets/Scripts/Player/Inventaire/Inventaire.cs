using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventaire : MonoBehaviour
{
    [SerializeField] private List<ItemData> allItems;
    [SerializeField] private UI_Inventaire inv;
    [SerializeField] private List<ItemData> itemDatas;

    public List<Image> spriteSlots;

    public GameObject tooltip;
    public TMP_Text tooltipText;
    public Transform tooltipRect;

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
            inv.UpdateUI();
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
            inv.UpdateUI();
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

    public List<ItemData> GetItemsData() {  return allItems; }

    public List<ItemData> GetItems() { return itemDatas; }
}
