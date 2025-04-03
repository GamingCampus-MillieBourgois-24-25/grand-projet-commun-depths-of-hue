using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private bool launchTimer;
    private float timer;
    [SerializeField] private Inventaire inventaire;
    [SerializeField] private UI_Inventaire inv;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (launchTimer)
        {
            timer += Time.deltaTime;
            if (timer >= 3) 
            {
                print("fini");
            }
        }
        
    }

    public void OnObjectClicked()
    {
        Debug.Log(gameObject.name + " a �t� cliqu� !");
        
        string gameObjectName = gameObject.name.ToLower();

        ItemData foundItem = inventaire.GetItems().Find(item => item.itemName.ToLower().Contains(gameObjectName));

        if (foundItem != null)
        {
            if (inventaire != null && inventaire.GetItemsData().Count < inv.spriteSlots.Count)
            {
                inventaire.Add(foundItem);
                Destroy(gameObject);

            }
            else
            {
                print("plus de place");
            }
        }
        else
        {
            // Si aucun �l�ment correspondant n'est trouv�
            Debug.Log("Aucun �l�ment trouv� avec un nom similaire � : " + gameObjectName);
        }
        
    }
}
