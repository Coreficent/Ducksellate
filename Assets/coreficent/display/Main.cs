﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    public Cell Cell;
    public Obstacle Obstacle;
    public GameObject Board;

    public static Cell[,] Cells;
    public static Queue<Cell> CellsActivated = new Queue<Cell>();

    private enum State { WIN, RUN };

    private string sceneCurrent;
    private const string MAIN = "Main";
    private const string TUTORIAL_ROTATE = "TutorialRotate";
    private const string TUTORIAL_REACT = "TutorialReact";

    void Start()
    {
        sceneCurrent = SceneManager.GetActiveScene().name;
        GenerateLevel();
        Log.Output("main initialized: ", sceneCurrent);
    }

    void Update()
    {
        switch (GetState())
        {
            case State.WIN:
                Debug.Log("won");
                switch (sceneCurrent)
                {
                    case TUTORIAL_ROTATE:
                        SceneManager.LoadScene(TUTORIAL_REACT);
                        break;
                    case TUTORIAL_REACT:
                        SceneManager.LoadScene(MAIN);
                        break;
                    case MAIN:
                        SceneManager.LoadScene("Replay");
                        break;
                    default:
                        Log.Output("unexpected scene");
                        break;
                }

                break;
            case State.RUN:
                while (CellsActivated.Count > 0)
                {
                    CellsActivated.Dequeue().React();
                }
                break;
            default:
                Debug.Log("unexpected state");
                break;
        }
    }

    private State GetState()
    {
        for (int x = 0; x < Cells.GetLength(0); ++x)
        {
            for (int y = 0; y < Cells.GetLength(1); ++y)
            {
                if (Cells[x, y] && !Cells[x, y].Tessellated())
                {
                    return State.RUN;
                }
            }
        }
        return State.WIN;
    }

    private void GenerateLevel()
    {
        GameObject currentBoard = Instantiate(Board);
        currentBoard.name = "Board";



        switch (sceneCurrent)
        {
            case (MAIN):
                int size = 7;
                Cells = new Cell[size, size];
                currentBoard.transform.position = new Vector3(-2f, 0f, 0f);
                for (int x = 0; x < size; ++x)
                {
                    for (int y = 0; y < size; ++y)
                    {
                        Cell currentCell = Instantiate(Cell, currentBoard.transform);
                        currentCell.X = x;
                        currentCell.Y = y;
                        currentCell.Randomize();
                        Cells[x, y] = currentCell;
                    }
                }
                break;
            case (TUTORIAL_ROTATE):
                Cells = new Cell[5, 5];
                currentBoard.transform.position = new Vector3(-3.5f, -2f, 0f);
                PopulateCell(currentBoard, 0, 4);
                PopulateCell(currentBoard, 1, 3);
                PopulateCell(currentBoard, 2, 2);
                PopulateCell(currentBoard, 3, 1);
                PopulateCell(currentBoard, 4, 0);
                PopulateRock(currentBoard);
                break;
            case (TUTORIAL_REACT):
                Cells = new Cell[7, 7];
                currentBoard.transform.position = new Vector3(-1f, -3f, 0f);
                PopulateCell(currentBoard, 1, 5);
                PopulateCell(currentBoard, 1, 6);
                PopulateCell(currentBoard, 0, 5);
                PopulateCell(currentBoard, 0, 6);

                PopulateCell(currentBoard, 3, 3);
                PopulateRock(currentBoard);
                break;
            default:
                Debug.Log("unexpected level");
                break;
        }
    }

    private void PopulateRock(GameObject board)
    {
        for (int x = 0; x < Cells.GetLength(0); ++x)
        {
            for (int y = 0; y < Cells.GetLength(1); ++y)
            {
                if (!Cells[x, y])
                {
                    Obstacle currentCell = Instantiate(Obstacle, board.transform);
                    currentCell.X = x;
                    currentCell.Y = y;
                }
            }
        }
    }

    private void PopulateCell(GameObject currentBoard, int x, int y)
    {
        Cell cellRotate;
        cellRotate = Instantiate(Cell, currentBoard.transform);
        cellRotate.X = x;
        cellRotate.Y = y;
        Cells[x, y] = cellRotate;
    }
}
