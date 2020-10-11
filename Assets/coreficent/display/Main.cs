using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public Cell cell;
    public GameObject board;

    public static Cell[,] cells;
    public static Queue<Cell> activatedCells = new Queue<Cell>();

    private enum State { win, run };

    

    // Start is called before the first frame update
    void Start()
    {
        Log.Output("main initialized");

        GenerateLevel();
    }
    // Update is called once per frame
    void Update()
    {
        switch (GetState())
        {
            case State.win:
                Debug.Log("won");
                SceneManager.LoadScene("Replay");
                break;
            case State.run:
                while (activatedCells.Count > 0)
                {
                    activatedCells.Dequeue().React();
                }
                break;
            default:
                Debug.Log("unexpected state");
                break;
        }
    }

    private State GetState()
    {
        for (int x = 0; x < cells.GetLength(0); ++x)
        {
            for (int y = 0; y < cells.GetLength(1); ++y)
            {
                if (!cells[x, y].Tessellated())
                {
                    return State.run;
                }
            }
        }
        return State.win;
    }

    private void GenerateLevel()
    {
        GameObject currentBoard = Instantiate(board);
        currentBoard.name = "Board";
        currentBoard.transform.position = new Vector3(-2f, 0, 0);

        int size = 2;
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
