using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enigme_Pillar : Enigme
{
    Pillar pillar = null;
    public GameObject pillarPrefab;
    public List<string> items;
    public List<string> OrderEnigme;
    public GameObject textPrefab;
    public GameObject popUp;
    public List<GameObject> sceneObjects;
    public float spacing = 2f;
    public Raycat ray;
    public List<Pillar> pillars;


    void Start()
    {
        if (popUp == null || textPrefab == null)
        {
            Debug.LogError("popUp ou textPrefab n'est pas assigné dans l'inspecteur.");
            return;
        }

        popUp.SetActive(false);
        SpawnPillars();
    }
    public void UpdatePopup()
    {
        pillar = ray.GetObj().GetComponent<Pillar>();
        if (pillar == null)
        {
            return;
        }

        ClearPopup();

        if (pillar.GetTake())
        {
            print("take");
            PopUpTake();
        }
        else
        {
            print("choice");
            PopUpItem();
        }
    }
    private void ClearPopup()
    {
        foreach (Transform child in popUp.transform)
        {
            if (child.gameObject != null)
            {
                Destroy(child.gameObject);
            }
        }
    }
    private void PopUpTake()
    {
        GameObject newText = Instantiate(textPrefab, popUp.transform);
        if (newText != null)
        {
            TextMeshProUGUI textMesh = newText.GetComponent<TextMeshProUGUI>();
            if (textMesh != null)
            {
                textMesh.enabled = true;
                textMesh.text = "reprendre";
                textMesh.ForceMeshUpdate();
                textMesh.tag = "Cube";

                Button textButton = newText.GetComponent<Button>();
                if (textButton == null)
                {
                    textButton = newText.AddComponent<Button>();
                }

                textButton.onClick.AddListener(OnResumeClicked);
            }

            RectTransform textRect = newText.GetComponent<RectTransform>();
            if (textRect != null)
            {
                textRect.anchoredPosition = Vector2.zero;
            }
        }

        RectTransform popUpRect = popUp.GetComponent<RectTransform>();
        if (popUpRect != null)
        {
            popUpRect.sizeDelta = new Vector2(200, 50);
        }
    }
    private void PopUpItem()
    {
        float totalHeight = 0f;
        float maxWidth = 0f;
        float textSpacing = 25f;
        float horizontalPadding = 20f;

        for (int i = 0; i < items.Count; i++)
        {
            string item = items[i];
            GameObject newText = Instantiate(textPrefab, popUp.transform);
            if (newText == null)
            {
                Debug.LogError("Échec de l'instanciation de textPrefab.");
                continue;
            }

            TextMeshProUGUI textMesh = newText.GetComponent<TextMeshProUGUI>();
            if (textMesh != null)
            {
                textMesh.enabled = true;
                textMesh.text = item;
                textMesh.ForceMeshUpdate();
                textMesh.tag = "Cube";

                totalHeight += textMesh.preferredHeight + textSpacing;
                if (textMesh.preferredWidth > maxWidth)
                {
                    maxWidth = textMesh.preferredWidth;
                }

                Button textButton = newText.GetComponent<Button>();
                if (textButton == null)
                {
                    textButton = newText.AddComponent<Button>();
                }
                textButton.enabled = true;
                textButton.onClick.AddListener(() => OnTextClicked(item));
            }
            else
            {
                Debug.LogError("Le composant TextMeshProUGUI n'a pas été trouvé sur le prefab instancié.");
                Destroy(newText);
                continue;
            }

            RectTransform textRect = newText.GetComponent<RectTransform>();
            if (textRect != null)
            {
                textRect.anchoredPosition = new Vector2(0, -totalHeight + (textMesh.preferredHeight / 2) + textSpacing * i);
            }
        }

        RectTransform popUpRect = popUp.GetComponent<RectTransform>();
        if (popUpRect != null)
        {
            popUpRect.sizeDelta = new Vector2(maxWidth + horizontalPadding, totalHeight);
        }
    }

    void OnResumeClicked()
    {
        items.Add(pillar.GetObj().name);
        sceneObjects.Add(pillar.GetObj());
        pillar.GetObj().transform.position = new Vector3(1000, 1000, 0);
        pillar.SetTake();
        pillar.SetObj(null);
        popUp.gameObject.SetActive(false);
    }


    void SpawnPillars()
    {
        if (pillarPrefab == null)
        {
            Debug.LogError("pillarPrefab n'est pas assigné.");
            return;
        }

        int count = items.Count;
        float offset = (count - 1) * spacing / 2f;

        for (int i = 0; i < count; i++)
        {
            float x = i * spacing - offset;
            Vector3 position = new Vector3(x, 0f, 0f);
            GameObject pillar = Instantiate(pillarPrefab, position, Quaternion.identity);
            if (pillar == null)
            {
                Debug.LogError("Échec de l'instanciation de pillarPrefab.");
                continue;
            }

            Pillar pillarScript = pillar.GetComponent<Pillar>();
            if (pillarScript == null)
            {
                pillarScript = pillar.AddComponent<Pillar>();
            }

            pillarScript.popup = popUp;
            pillarScript.spawner = this;
            pillarScript.SetId(OrderEnigme[i]);
            pillars.Add(pillarScript);
        }
    }
    public override void UpdateEnigme(float deltaTime)
    {
        Success();
    }

    private void Update()
    {
        if (isStarted)
        {

            UpdateEnigme(Time.deltaTime);
        }
    }
    protected override void Success()
    {
        foreach (var pillar in pillars)
        {
            if (pillar.GetObj() == null || pillar.GetObj().name != pillar.GetId())
            {
                return;
            }
        }
        print("c'est win");
        isStarted = false;
        isResolved = true;
        OnSuccess?.Invoke();
    }

    void OnTextClicked(string itemName)
    {
        List<GameObject> objectsToKeep = new List<GameObject>();

        foreach (GameObject obj in sceneObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                objectsToKeep.Add(obj);
            }
        }

        GameObject targetObject = objectsToKeep.Find(obj => obj != null && obj.name == itemName);
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            if (ray != null)
            {
                targetObject.transform.position = ray.GetPosition();
            }
            objectsToKeep.Remove(targetObject);
            items.Remove(itemName);
            UpdatePopup();
            popUp.SetActive(false);
            sceneObjects.Clear();
            sceneObjects.AddRange(objectsToKeep);
            pillar.SetTake();
            pillar.SetObj(targetObject);
        }
        else
        {
            Debug.LogWarning($"Aucun objet nommé '{itemName}' n'a été trouvé dans le tableau sceneObjects.");
        }


    }
}
