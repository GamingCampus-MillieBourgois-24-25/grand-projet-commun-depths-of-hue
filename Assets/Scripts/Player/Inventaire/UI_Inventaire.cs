using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventaire : MonoBehaviour
{
    [SerializeField] private Inventaire inv;
    public List<Image> spriteSlots;
    private List<ItemData> allItems;
    public GameObject tooltip;
    public TMP_Text tooltipText;
    public RectTransform tooltipRect;
    public Image inspectionImage;
    

    void Start()
    {
        allItems = inv.GetItemsData();
        UpdateUI();
        HideTooltip();
        inspectionImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
        {
            HideInspectionImage();
        }
    }

    public void UpdateUI()
    {
        List<Sprite> sprites = inv.GetAllSprites();
        for (int i = 0; i < spriteSlots.Count; i++)
        {
            if (sprites.Count > i && sprites[i] != null)
            {
                spriteSlots[i].sprite = sprites[i];
                AddTooltip(spriteSlots[i], i);
                AddClickEvent(spriteSlots[i], i);
            }
            else
            {
                spriteSlots[i].sprite = null;
            }
        }
    }

    public void AddTooltip(Image slot, int index)
    {
        EventTrigger trigger = slot.gameObject.GetComponent<EventTrigger>() ?? slot.gameObject.AddComponent<EventTrigger>();
        trigger.triggers.Clear();

        EventTrigger.Entry entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((data) => ShowTooltip(index, slot));

        EventTrigger.Entry entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((data) => HideTooltip());

        trigger.triggers.Add(entryEnter);
        trigger.triggers.Add(entryExit);
    }

    public void AddClickEvent(Image slot, int index)
    {
        Button button = slot.gameObject.GetComponent<Button>() ?? slot.gameObject.AddComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => InspectItem(index));
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

    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    public void InspectItem(int index)
    {
        if (index < allItems.Count && allItems[index].type.ToString() == "inspectable")
        {
            inspectionImage.sprite = inv.GetAllSprites()[index];
            inspectionImage.gameObject.SetActive(true);
        }
    }

    public void HideInspectionImage()
    {
        inspectionImage.gameObject.SetActive(false);
    }

    private bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}
