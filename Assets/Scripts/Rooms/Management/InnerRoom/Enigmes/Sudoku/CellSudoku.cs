using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CellSudoku : MonoBehaviour
{
    public int x;

    public int y;
    
    public bool isEditable = true;

    [SerializeField] private GameObject notEditableImage;

    public void UpdateNotEditable()
    {
        if (!isEditable)
        {
            notEditableImage.SetActive(true);
        }
           
    }

    public void Test()
    {
        Debug.Log(x +" , " + y);
    }

    public void SetPosition(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public void GetCellPosition(out int _x, out int _y)
    {
        _x = x;
        _y = y;
    }

    public void PlayChoice()
    {
        if (isEditable)
        {
            Sudoku.Instance.PlayHand(x, y, this);
        }
        else
        {
            Debug.Log("This cell is not editable.");
        }
    }
}
