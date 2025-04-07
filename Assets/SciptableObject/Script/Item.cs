using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    public string itemDescription;
    public enum itemType { none, inspectable, Enigme, };
    public itemType type;
}

