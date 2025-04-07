using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventaire : MonoBehaviour
{
    [SerializeField] private List<ItemData> inventaire;
    [SerializeField] private UI_Inventaire inv;
    [SerializeField] private List<ItemData> itemDatas;
    [SerializeField] private ParticleSystem part;
    [SerializeField] private Save save;
    private void Awake()
    {
        save.LoadGame();
    }

    void Start()
    {
        string searchTerm = "perle";
        List<ItemData> matchingItems = FindItemsByPartialName( searchTerm);

        foreach (ItemData item in matchingItems)
        {
            Debug.Log("Objet trouv� : " + item.itemName);
        }
        
    }

    public void Add(ItemData item, GameObject obj)
    {
        if (!inventaire.Contains(item))
        {
            inventaire.Add(item);
            inv.UpdateUI();
            Destroy(obj);
            part.transform.position = obj.transform.position;
            part.Play();
            Debug.Log(item.itemName + " ajout� � l'inventaire.");
            save.SaveGame();
        }
        else
        {
            Debug.Log(item.itemName + " est d�j� dans l'inventaire.");
        }
    }

    public void Remove( ItemData item)
    {
        if (inventaire.Contains(item))
        {
            inventaire.Remove(item);
            inv.UpdateUI();
            Debug.Log(item.itemName + " retir� de l'inventaire.");
        }
        else
        {
            Debug.Log(item.itemName + " n'est pas dans l'inventaire.");
        }
    }

    public List<ItemData> FindItemsByPartialName( string partialName)
    {
        List<ItemData> foundItems = inventaire.Where(item => item.itemName.IndexOf(partialName, System.StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        if (foundItems.Count == 0)
        {
            throw new System.Exception("Aucun objet trouv� contenant : " + partialName);
        }
        return foundItems;
    }

    public ItemData GetItemByName(string itemName)
    {
        ItemData item = inventaire.FirstOrDefault(item => item.itemName.Equals(itemName, System.StringComparison.OrdinalIgnoreCase));
        if (item == null)
        {
            throw new System.Exception("Aucun objet trouv� avec le nom : " + itemName);
        }
        return item;
    }

    public List<Sprite> GetAllSprites()
    {
        if (inventaire.Count == 0)
        {
            throw new System.Exception("L'inventaire est vide, aucun sprite disponible.");
        }
        List<Sprite> sprites = inventaire.Select(item => item.itemSprite).Where(sprite => sprite != null).ToList();
        if (sprites.Count == 0)
        {
            throw new System.Exception("Aucun sprite valide trouv� dans l'inventaire.");
        }
        return sprites;
    }

    public List<ItemData> GetInventaire() {  return inventaire; }
    public void SetInventaire(List<ItemData> _inventaire) { inventaire = _inventaire; }
    public List<ItemData> GetItems() { return itemDatas; }
}
