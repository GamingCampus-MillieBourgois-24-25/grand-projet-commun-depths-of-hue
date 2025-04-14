using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

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
public class Sudoku : Enigme
{
    public static Sudoku Instance;
    
    private SudokuPlay[,] sudokuGrid;
    [SerializeField] private int gridSize = 4;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Canvas canvasEnigme;

    [SerializeField] private GameObject startPosition;

    private int currentCount = 1;
    private int colorId = 0;
    [SerializeField] private TMP_Text choiceCount;
    [SerializeField] private Image choiceColorImage;

    [SerializeField]
    private List<Color> colorList = new List<Color>();
    public override void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
            base.Initialize();
        }

        InstantiateGrid();
        float pos = -(gridSize / 2 * 110);
        startPosition.transform.localPosition = new Vector3(pos, pos, 0);
        Debug.Log("YALALALAOOOOOOOOOOOOOO");
    }
    
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
                if (rowCell.countPiece < 1 || rowCell.countPiece > 4 ||
                    !numbersInRow.Add(rowCell.countPiece) ||
                    !colorsInRow.Add(rowCell.color))
                {
                    return false;
                }

                SudokuPlay colCell = sudokuGrid[j, i];
                if (colCell.countPiece < 1 || colCell.countPiece > 4 ||
                    !numbersInCol.Add(colCell.countPiece) ||
                    !colorsInCol.Add(colCell.color))
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void PlayHand(int row, int col, CellSudoku cell)
    {
        if (row >= 0 && row < gridSize && col >= 0 && col < gridSize)
        {
            SudokuPlay play = new SudokuPlay();
            play.countPiece = currentCount;
            play.color = (ColorSudoku)colorId;
            sudokuGrid[row, col] = play;
            cell.GetComponent<Image>().color = colorList[colorId];
            cell.GetComponentInChildren<TMP_Text>().text = currentCount.ToString();
            if (IsGameWon())
            {
                Debug.Log("You won the game!");
            }
        }
    }

    public void InstantiateGrid()
    {
        InitializeGrid();
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(0,0,0), Quaternion.identity);
                cell.transform.SetParent(startPosition.transform);
                cell.transform.localPosition = new Vector3(i * 110, j * 110, 0);
                cell.GetComponent<CellSudoku>().x = i;
                cell.GetComponent<CellSudoku>().y = j;
            }
        }
    }

    public void ChangePlay(int buttonId, bool isColor)
    {
        if (isColor)
        {
            colorId = buttonId;
            choiceColorImage.color = colorList[colorId];
        }
        else
        {
            currentCount = buttonId+1;
            choiceCount.text = currentCount.ToString();
        }
    }
    
    
}
