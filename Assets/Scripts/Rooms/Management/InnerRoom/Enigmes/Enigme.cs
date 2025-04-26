using System.Collections.Generic;
using UnityEngine;

public abstract class Enigme : MonoBehaviour
{
    public bool isResolved = false;
    protected bool isStarted = false;
    public int hintLeft = 3;
    private int hintUsed = 0;

    [SerializeField] protected GameObject itemsContainer;

    public DialogueGroupKey enigmeDialogKey;


    public List<GameObject> objectsInEnigme;  // Every object possibly usable for the enigme

    public delegate void EnigmeEventHandler();

    public event EnigmeEventHandler OnSuccess;
    public event EnigmeEventHandler OnFail;


    /// <summary>
    /// This function must be called first. It will be used for the enigme setup.
    /// </summary>
    public virtual void Initialize()
    {
        isStarted = true;

        if (itemsContainer != null)
        {
            objectsInEnigme = new List<GameObject>();

            foreach (Transform child in itemsContainer.transform)
            {
                objectsInEnigme.Add(child.gameObject);
            }
        }      
    }

    public virtual void UpdateEnigme(float deltaTime)
    {
        //Code updated if needed.
    }

    /// <summary>
    /// Enigme succes instructions.
    /// </summary>
    protected virtual void Success()
    {
        isStarted = false;
        isResolved = true;
        
        OnSuccess?.Invoke();
    }



    /// <summary>
    /// Enigme failed instructions.
    /// </summary>
    protected virtual void Fail()
    {
        isStarted = false;
        OnFail?.Invoke();
    }

    /// <summary>
    /// Getter for the resolution state
    /// </summary>
    /// <returns></returns>
    public bool GetIsResolved()
    {
        return isResolved;
    }

    /// <summary>
    /// Function used to check the item 
    /// </summary>
    /// <param name="item"></param>
    public virtual void CheckItem(GameObject item)
    {
        //Content//
        Debug.Log("sp");
    }
}
