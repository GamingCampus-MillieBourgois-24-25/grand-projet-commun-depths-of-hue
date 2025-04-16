using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enigme_Doble : Enigme
{
    private GameObject firstSelected;
    private GameObject secondSelected;
    private int nbDouble;
    [SerializeField] List<GameObject> ItemsDouble;
    void Start()
    {
        
    }

    public void ObjectClicked(GameObject obj)
    {
        if (obj == firstSelected) return;

        if (firstSelected == null)
        {
            firstSelected = obj;
        }
        else
        {
            secondSelected = obj;
            CheckObj();
        }
    }

    public void CheckObj()
    {
        ItemDouble firstObj = firstSelected.GetComponent<ItemDouble>();
        ItemDouble secondObj = secondSelected.GetComponent<ItemDouble>();
        if(firstObj == null || secondObj == null)
        {
            return;
        }
        if (firstObj.GetId() == secondObj.GetId())
        {
            firstSelected.SetActive(false);
            secondSelected.SetActive(false);
            nbDouble--;
        }
        else
        {
            firstSelected = null;
            secondSelected = null;
        }
    }

    protected override void Success()
    {
        if(nbDouble == 0)
        {
            isStarted = false;
            isResolved = true;
            OnSuccess?.Invoke();
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
}
