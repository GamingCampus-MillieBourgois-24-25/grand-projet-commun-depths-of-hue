using System;
using System.Collections.Generic;
using UnityEngine;

public class SudokuGenerator : MonoBehaviour
{
    private SudokuPlay[,] grid = new SudokuPlay[4, 4];
    private System.Random random = new System.Random();

    public SudokuPlay[,] CreateSolvedGrid()
    {
        int maxAttempts = 1000; // Increased from 100
        int attempts = 0;
        
        while (attempts++ < maxAttempts)
        {
            // Initialize empty grid
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    grid[i, j] = new SudokuPlay { countPiece = 0, color = ColorSudoku.Red };

            // Try to solve
            if (SolveCell(0, 0, true))
            {
                PrintGrid();
                return grid;
            }
        }

        Debug.LogError($"Failed after {maxAttempts} attempts");
        return null;
    }

    private bool SolveCell(int row, int col, bool randomize)
    {
        // If we've filled all rows, we're done
        if (row >= 4) return true;
        
        // Move to next row when column is complete
        if (col >= 4) return SolveCell(row + 1, 0, randomize);

        // Skip already filled cells
        if (grid[row, col].countPiece != 0)
            return SolveCell(row, col + 1, randomize);

        // Create and shuffle options
        List<int> numbers = new List<int> { 1, 2, 3, 4 };
        List<ColorSudoku> colors = new List<ColorSudoku> { ColorSudoku.Red, ColorSudoku.Purple, ColorSudoku.Blue, ColorSudoku.Yellow };

        if (randomize)
        {
            FisherYatesShuffle(numbers);
            FisherYatesShuffle(colors);
        }

        // Try all combinations
        foreach (int num in numbers)
        {
            foreach (ColorSudoku color in colors)
            {
                if (IsValidPlacement(row, col, num, color))
                {
                    grid[row, col] = new SudokuPlay { countPiece = num, color = color };

                    if (SolveCell(row, col + 1, randomize))
                        return true;

                    // Backtrack
                    grid[row, col] = new SudokuPlay { countPiece = 0, color = ColorSudoku.Red };
                }
            }
        }

        return false;
    }

    private bool IsValidPlacement(int row, int col, int number, ColorSudoku color)
    {
        // Check row and column
        for (int i = 0; i < 4; i++)
        {
            // Skip current cell
            if (i == col && grid[row, i].countPiece == 0) continue;
            if (i == row && grid[i, col].countPiece == 0) continue;

            // Check row
            if (grid[row, i].countPiece == number || grid[row, i].color == color)
                return false;

            // Check column
            if (grid[i, col].countPiece == number || grid[i, col].color == color)
                return false;
        }

        // Check 2x2 subgrid
        int boxRow = row / 2 * 2;
        int boxCol = col / 2 * 2;

        for (int r = boxRow; r < boxRow + 2; r++)
        {
            for (int c = boxCol; c < boxCol + 2; c++)
            {
                // Skip current cell
                if (r == row && c == col) continue;
                if (grid[r, c].countPiece == 0) continue;

                if (grid[r, c].countPiece == number || grid[r, c].color == color)
                    return false;
            }
        }

        return true;
    }

    private void FisherYatesShuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private void PrintGrid()
    {
        string output = "Valid Solution:\n";
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                output += $"{grid[i, j].countPiece}/{grid[i, j].color}".PadRight(15);
            }
            output += "\n";
        }
        Debug.Log(output);
    }

    private void Start()
    {
        CreateSolvedGrid();
    }
}