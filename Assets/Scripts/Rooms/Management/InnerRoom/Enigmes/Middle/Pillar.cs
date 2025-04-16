using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Pillar : MonoBehaviour
{
    public GameObject popup;
    private GameObject Obj;
    public Enigme_Pillar spawner;
    private bool isObj;
    private string Id;
    

    public void OnObjectClicked()
    {
        spawner.UpdatePopup();
        if (popup == null || gameObject == null)
        {
            Debug.LogWarning("Popup ou targetObject est null !");
            return;
        }

        popup.SetActive(true);

        Vector3 worldPosition = gameObject.transform.position + Vector3.up * 2f; 
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        RectTransform popupRect = popup.GetComponent<RectTransform>();
        popupRect.position = screenPosition;
    }


    void OnEnable()
    {
        Raycat.OnClickOnNothing += HandleClickOnNothing;
    }

    void HandleClickOnNothing()
    {
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;
        popup.SetActive(false);
    }

    public void SetTake()
    {
        isObj = !isObj;
    }

    public void SetObj(GameObject _obj) { Obj = _obj; }
    public GameObject GetObj() { return Obj; }

    public bool GetTake() { return isObj; }

    public void SetId(string _id) { Id = _id; }
    public string GetId() { return Id; }
}