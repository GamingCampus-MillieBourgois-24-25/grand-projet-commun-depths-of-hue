using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowsTrigger : MonoBehaviour
{
    [SerializeField] private GestionCadre cadre;
    [SerializeField] private GameObject targetCadre;
    private void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void OnPointerDownDelegate(PointerEventData data)
    {
        Debug.Log("clicked");
        cadre.NavigateCadre(targetCadre);
    }
}
