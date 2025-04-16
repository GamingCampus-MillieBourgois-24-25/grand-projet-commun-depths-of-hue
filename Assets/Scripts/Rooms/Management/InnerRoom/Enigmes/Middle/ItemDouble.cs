using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDouble : MonoBehaviour
{
    [SerializeField] private int ID;
    public Enigme_Doble enigmeD;

    public void OnObjectClicked()
    {
        enigmeD.ObjectClicked(gameObject);
    }

    public int GetId() {  return ID; }
}
