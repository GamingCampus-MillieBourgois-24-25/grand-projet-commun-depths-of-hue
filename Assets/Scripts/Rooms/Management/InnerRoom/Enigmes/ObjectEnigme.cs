using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEnigme : MonoBehaviour
{

    [ContextMenu("Debug")]
    public void OnObjectClicked()
    {
       
        if (Enigme_FindObjects.Instance != null)
        {
            Enigme_FindObjects.Instance.CheckItem(this.gameObject);
        }
        else
        {
            Debug.Log("no instance");
        }


    }
}
