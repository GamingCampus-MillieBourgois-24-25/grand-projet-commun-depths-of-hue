using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSudoku : MonoBehaviour
{
    public int x;

    public int y;

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
        Sudoku.Instance.PlayHand(x,y, this);
    }
}
