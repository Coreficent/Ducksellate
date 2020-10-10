using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Cell[,] cells;
    public static Queue<Cell> activatedCells = new Queue<Cell>();

    public Cell cell;
    public GameObject board;

    // Start is called before the first frame update
    void Start()
    {
        Log.Output("main initialized");

        GenerateLevel();
    }
    // Update is called once per frame
    void Update()
    {
        while (activatedCells.Count > 0)
        {
            activatedCells.Dequeue().React();
        }
        if (Won())
        {
            Log.Output("won");
        }
    }

    private bool Won()
    {
        for (int x = 0; x < cells.GetLength(0); ++x)
        {
            for (int y = 0; y < cells.GetLength(1); ++y)
            {
                if (!cells[x, y].Tessellated())
                {
                    return false;
                }
            }
        }
        return true;
    }


    private void GenerateLevel()
    {
        GameObject currentBoard = Instantiate(board);
        currentBoard.name = "Board";
        currentBoard.transform.position = new Vector3(-2f, 0, 0);

        int size = 4;
        cells = new Cell[size, size];
        for (int x = 0; x < size; ++x)
        {
            for (int y = 0; y < size; ++y)
            {
                Cell currentCell = Instantiate(cell, currentBoard.transform);
                currentCell.X = x;
                currentCell.Y = y;
                cells[x, y] = currentCell;
            }
        }
    }
}
