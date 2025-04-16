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
    None,  // 0
    Red,   // 1
    Purple,  // 2
    Blue,// 3
    Yellow // 4
}
public class Sudoku : Enigme
{
    public static Sudoku Instance;
    
    private SudokuPlay[,] sudokuGrid;
    [SerializeField] private int gridSize = 4;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Canvas canvasEnigme;

    [SerializeField] private GameObject startPosition;

    private int currentCount = 0;
    private int colorId = 0;
    [SerializeField] private TMP_Text choiceCount;
    [SerializeField] private Image choiceColorImage;

    [SerializeField]
    private List<Color> colorList = new List<Color>();

    [SerializeField] private int numberOfBlankCases = 8;
    
    private SudokuGenerator sudokuGenerator;
    
    public override void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
            base.Initialize();
        }

        sudokuGenerator = GetComponent<SudokuGenerator>();
        var fullSolution = sudokuGenerator.CreateSolvedGrid();

        sudokuGrid = new SudokuPlay[gridSize, gridSize];
        System.Random rand = new System.Random();

        int totalCells = gridSize * gridSize;

        List<(int, int)> allPositions = new List<(int, int)>();
        for (int i = 0; i < gridSize; i++)
        for (int j = 0; j < gridSize; j++)
            allPositions.Add((i, j));
        
        for (int i = allPositions.Count - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (allPositions[i], allPositions[j]) = (allPositions[j], allPositions[i]);
        }
        
        HashSet<(int, int)> blankPositions = new HashSet<(int, int)>(allPositions.GetRange(0, numberOfBlankCases));


        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (blankPositions.Contains((i, j)))
                {
                    sudokuGrid[i, j] = new SudokuPlay
                    {
                        countPiece = 0,
                        color = ColorSudoku.None
                    };
                }
                else
                {
                    sudokuGrid[i, j] = fullSolution[i, j];
                }
            }
        }


        InstantiateGrid();
        float pos = gridSize / 2 * 110;
        startPosition.transform.localPosition = new Vector3(-pos, pos, 0);
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

                if (rowCell.color == ColorSudoku.None || colCell.color == ColorSudoku.None)
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
            if (currentCount != 0)
            {
                cell.GetComponentInChildren<TMP_Text>().text = currentCount.ToString();
            }
            else
            {
                cell.GetComponentInChildren<TMP_Text>().text = "";
            }
            ResetChoice();
            if (IsGameWon())
            {
                Debug.Log("You won the game!");
            }
        }
    }

    public void InstantiateGrid()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                GameObject cell = Instantiate(cellPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                cell.transform.SetParent(startPosition.transform);
                cell.transform.localPosition = new Vector3(j * 110, -i * 110, 0);
                var cellScript = cell.GetComponent<CellSudoku>();
                cellScript.x = i;
                cellScript.y = j;

                int colorIndex = (int)sudokuGrid[i, j].color;

                var cellImage = cell.GetComponent<Image>();
                if (colorIndex >= 0 && colorIndex < colorList.Count)
                {
                    cellImage.color = colorList[colorIndex];
                }
                else
                {
                    cellImage.color = Color.white;
                }

                var text = cell.GetComponentInChildren<TMP_Text>();
                if (sudokuGrid[i, j].countPiece != 0)
                {
                    text.text = sudokuGrid[i, j].countPiece.ToString();
                    cellScript.isEditable = false;
                    cellScript.UpdateNotEditable();
                }
                else
                {
                    text.text = "";
                    cellScript.isEditable = true; 
                }
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
            currentCount = buttonId;
            if (currentCount != 0)
            {
                choiceCount.text = currentCount.ToString();
            }
            else
            {
                choiceCount.text = "";
            }
        }
    }

    private void ResetChoice()
    {
        currentCount = 0;
        colorId = 0;
        ChangePlay(0,false);
        ChangePlay(0,true);
    }
    
}
