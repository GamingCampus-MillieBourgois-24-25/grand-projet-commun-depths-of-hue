using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SudokuPlay
{
    [Range(1, 4)]
    public int countPiece;
    public ColorSudoku color;
}

public enum ColorSudoku
{
    Red,
    Purple,
    Blue,
    Yellow
}
public class Sudoku : MonoBehaviour
{
    private SudokuPlay[,] sudokuGrid;
    [SerializeField] private int gridSize = 4;

    private void InitializeGrid()
    {
        sudokuGrid = new SudokuPlay[gridSize,gridSize];
    }

    public bool IsGameWon()
    {
        if (sudokuGrid == null) return false;

        for (int i = 0; i < gridSize; i++)
        {
            var numbersInRow = new HashSet<int>();
            var numbersInCol = new HashSet<int>();
            var colorsInRow = new HashSet<ColorSudoku>();
            var colorsInCol = new HashSet<ColorSudoku>();

            for (int j = 0; j < gridSize; j++)
            {
                SudokuPlay rowCell = sudokuGrid[i, j];
                if (rowCell.countPiece < 1 || rowCell.countPiece > gridSize ||
                    !numbersInRow.Add(rowCell.countPiece) ||
                    !colorsInRow.Add(rowCell.color))
                {
                    return false;
                }

                SudokuPlay colCell = sudokuGrid[j, i];
                if (colCell.countPiece < 1 || colCell.countPiece > gridSize ||
                    !numbersInCol.Add(colCell.countPiece) ||
                    !colorsInCol.Add(colCell.color))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void PlayHand(int row, int col, SudokuPlay play)
    {
        if (row >= 0 && row < gridSize && col >= 0 && col < gridSize)
        {
            sudokuGrid[row, col] = play;

            if (IsGameWon())
            {
                Debug.Log("You won the game!");
            }
        }
    }


}
