using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventaire : MonoBehaviour
{
    public List<ItemData> allItems;

    public List<Image> spriteSlots;

    public GameObject tooltip; // UI du pop-up
    public TMP_Text tooltipText; // Texte du pop-up
    public Transform tooltipRect;

    void Start()
    {
        string searchTerm = "perle";
        List<ItemData> matchingItems = FindItemsByPartialName(searchTerm);

        foreach (ItemData item in matchingItems)
        {
            Debug.Log("Objet trouvé : " + item.itemName);
        }
        UpdateUI();
        HideTooltip();
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

    public void UpdateUI()
    {
        List<Sprite> sprites = GetAllSprites();

        for (int i = 0; i < spriteSlots.Count; i++)
        {
            if (sprites.Count > i && sprites[i] != null)
            {
                spriteSlots[i].sprite = sprites[i];
                AddTooltip(spriteSlots[i], i);

            }
            else
            {
                spriteSlots[i].sprite = null;
            }
        }
    }

    public void AddTooltip(Image slot, int index)
    {
        EventTrigger trigger = slot.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = slot.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((data) => ShowTooltip(index, slot));

        EventTrigger.Entry entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((data) => HideTooltip());

        trigger.triggers.Clear();
        trigger.triggers.Add(entryEnter);
        trigger.triggers.Add(entryExit);
    }

    public void ShowTooltip(int index, Image slot)
    {
        if (index < allItems.Count)
        {
            tooltipText.text = allItems[index].itemName + "\n" + allItems[index].itemDescription;
            tooltip.SetActive(true);

            Vector3 slotPosition = slot.transform.position;
            tooltipRect.position = new Vector3(slotPosition.x, slotPosition.y + 100, slotPosition.z);
        }
    }

    void HideTooltip()
    {
        tooltip.SetActive(false);
    }
}
